namespace Hyperion.ExploreCameraNames

open System.Collections.Generic
open System.IO

open ShellProgressBar

open Hyperion.ExploreCameraNames.Utils
open Hyperion.ExploreCameraNames.DataRetrieval
open Hyperion.ExploreCameraNames.GroupBrands
open Hyperion.ExploreCameraNames.SegmentProdRanges

module Main =
    let testParsing() =
        let ricohCamera =
            let ricohDict =
                "..\\2013_camera_specs\\buy.net\\5955.json"
                |> deserialiseJsonToDictAsync
                |> Async.RunSynchronously
        
            [|("buy.net//5955", ricohDict)|]

        let multiCamera =
            ricohCamera
            |> countDPReviewBrands
            |> cameraIdsMultiBrand

        groupedCameraIdsMultiBrand multiCamera
        |> printfn "%A"

    let brandStats() =
        // let ticks = 3

        // use pbar = new ProgressBar(ticks, "loading all files", pbOptions)

        let allFiles = allDicts()

        //pbar.Tick "determining brands in files"
        let brandedCameraIds =
            allFiles
            |> countDPReviewBrands

        //pbar.Tick "evaluating number of files with no brand"
        let numberOfNoBrand =
            brandedCameraIds
            |> cameraIdsNoBrands
            |> List.length

        //pbar.Tick "evaluating number of files with only one brand"
        let numberOfExclusive =
            brandedCameraIds
            |> cameraIdsOneBrand
            |> List.length

        Array.length allFiles
        |> printfn "Number of files: %d"
        // |> pbar.WriteLine

        numberOfNoBrand
        |> printfn "Number of files with no DPReview brand: %d"
        // |> pbar.WriteLine

        numberOfExclusive
        |> printfn "Number of files with one DPReview brand: %d"
        // |> pbar.WriteLine

        (Array.length allFiles - (numberOfNoBrand + numberOfExclusive))
        |> printfn "Numbers of outstanding files: %d"
        // |> pbar.WriteLine

    let serialiseBrands() =
        //let ticks = 7

        //use pbar = new ProgressBar(ticks, "loading all files", pbOptions)

        let allFiles = allDicts()

        //pbar.Tick "determining brands in files"
        let brandedCameraIds =
            allFiles
            |> countDPReviewBrands

        //pbar.Tick "determining files with no brands"
        let noBrand =
            brandedCameraIds
            |> cameraIdsNoBrands

        //pbar.Tick "determining files with one brand"
        let oneBrand =
            brandedCameraIds
            |> cameraIdsOneBrand

        //pbar.Tick "determining files with multiple brands"
        let multiBrand =
            brandedCameraIds
            |> cameraIdsMultiBrand

        //pbar.Tick "converting brand information into dictionary"
        let dictionary =
            groupCameraSpecsByBrand oneBrand noBrand multiBrand

        //pbar.Tick "serialising data"
        dictionary
        |> serialiseData "groupedByBrand.json"

    let brandProductRanges (cameraBrand: BrandRanges) = 
        let filepath = Path.Combine("..", "ScrapyScrape", "dpreview_cams.json")

        let dprSpecs =
            filepath
            |> deserialiseDpreviewSpecs

        let models =
            dprSpecs
            |> modelsOfBrand cameraBrand.Name

        models
        |> groupByRange cameraBrand.Ranges
        |> List.map (fun (x, ys) -> x, List.map separateAltNames ys)
        |> printfn "%A"

    let searchForProductRanges (cameraBrand: BrandRanges) =
        let filepath = Path.Combine("..", "ScrapyScrape", "dpreview_cams.json")

        let allFiles =
            allDicts()

        let olympusFiles =
            allFiles
            |> countDPReviewBrands
            |> cameraIdsGivenBrand cameraBrand.Name
            |> dictsOfIds allFiles
            |> Array.map (fun (cameraId, cameraDict) ->
                cameraId, cameraDict.["<page title>"] |> string)

        let modelRegexes =
            filepath
            |> deserialiseDpreviewSpecs
            |> modelsOfBrand cameraBrand.Name
            |> List.map (fun x ->
                let nameRegexes =
                    x
                    |> separateAltNames
                    |> tokenizedModelNames
                x, nameRegexes)

        [ for modelName, modelNameRegex in modelRegexes do
            let matchingIds =
                olympusFiles
                |> Array.filter (fun (_, pageTitle) ->
                    modelNameRegex
                    |> Array.exists (fun rx -> rx.IsMatch(pageTitle)))
                |> Array.map fst
            modelName, matchingIds ]

    let testMatchingSubsets (cameraBrand: BrandRanges) =
        let regexes =
            Path.Combine("..", "ScrapyScrape", "dpreview_cams.json")
            |> deserialiseDpreviewSpecs
            |> modelsOfBrand cameraBrand.Name
            |> List.map (fun x ->
                let nameRegexes =
                    x.ToLower()
                    |> separateAltNames
                    |> tokenizedModelNames
                x, nameRegexes)

        // regexes
        // |> List.take 10
        // |> List.map snd
        // |> List.iter (fun x -> x |> Array.iter (fun y -> y |> printfn "%A"))

        regexes
        |> mapOfSubsetRegexes
        |> Map.filter (fun _ xs -> xs |> List.isEmpty |> not)
        |> printfn "%A"

    let serialiseProductRanges() =
        Path.Combine("..", "ScrapyScrape", "dpreview_cams.json")
        |> groupedModelsToDicts
        |> serialiseData "camera-ranges.json"

    let serialiseSearchVerbose listings brandCount filenameSuffix =
        let filename =
            filenameSuffix
            |> sprintf "equivalent-camera-verbose-%s.json"

        Path.Combine("..", "ScrapyScrape", "dpreview_cams.json")
        |> searchAndGroupVerbose listings brandCount
        |> serialiseData filename

        printfn "saved %s" filename

    let serialiseSearchMinimal listings brandCount filenameSuffix =
        let filename =
            filenameSuffix
            |> sprintf "equivalent-cameras-%s.json"

        Path.Combine("..", "ScrapyScrape", "dpreview_cams.json")
        |> searchAndGroupMinimal listings brandCount
        |> serialiseData filename

        printfn "saved %s" filename

    let productRange() =
        let filepath = Path.Combine("..", "ScrapyScrape", "dpreview_cams.json")

        let brands = 
          [  ]

        // let allFiles =
        //     allDicts()

        // let olympusFiles =
        //     allFiles
        //     |> countDPReviewBrands
        //     |> cameraIdsGivenBrand "Olympus"
        //     |> SegmentProdRanges.dictsOfBrand allFiles

        let dprSpecs =
            filepath
            |> deserialiseDpreviewSpecs

        // SegmentProdRanges.OlympusProcessing.allOlympusModels dprSpecs
        // |> SegmentProdRanges.cameraDictsMatchingModel olympusFiles
        // |> List.map (fun (x, ys) -> x, ys |> List.length)
        // // |> List.filter (fun (x, _) -> x <> "Unmatched")
        // // |> List.sumBy snd
        // |> printTuples
        // // |> List.last
        // // |> snd
        // // |> List.take 10
        // // |> printfn "%A"

        let models =
            dprSpecs
            |> modelsOfBrand olympus.Name

        models
        |> groupByRange olympus.Ranges
        |> printfn "%A"

    [<EntryPoint>]
    let main argv =
        let allFiles =
            allDicts()
        let dpreviewBrandCount =
            allFiles
            |> countDPReviewBrands

        serialiseSearchMinimal allFiles dpreviewBrandCount "7"
        serialiseSearchVerbose allFiles dpreviewBrandCount "7"

        0 // return an integer exit code
