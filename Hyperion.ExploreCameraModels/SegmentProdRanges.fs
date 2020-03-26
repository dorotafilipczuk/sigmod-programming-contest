namespace Hyperion.ExploreCameraNames

open System.Collections.Generic
open System.Text.RegularExpressions

open Utils

module SegmentProdRanges =

    type BrandRanges =
      { Name: string
        Ranges: string list }

    [<AutoOpen>]
    module CameraBrandRanges =
        let olympus =
          { Name = "Olympus"
            Ranges = 
              [ "OM-D"
                "PEN"
                "Stylus Touch"
                "Stylus"
                "Tough"
                "TG-"
                "SH-"
                "XZ-"
                "VH-"
                "VR-"
                "SP-"
                "SZ-"
                "VG-"
                "FE-"
                "C-"
                "D-"
                "E-" ] }

        let canon =
          { Name = "Canon"
            Ranges =
              [ "EOS M"
                "EOS Rebel"
                "EOS R"
                "EOS D"
                "EOS x"
                "EOS-5D"
                "EOS-1D"
                "PowerShot SX"
                "PowerShot SD"
                "PowerShot G"
                "PowerShot D"
                "PowerShot A"
                "PowerShot S"
                "ELPH"
                "PowerShot Pro" ] }

        let fujifilm =
          { Name = "Fujifilm"
            Ranges =
              [ "XP"
                "X-T"
                "X-A"
                "X-Pro"
                "X-"
                "GFX"
                "XF"
                "X-H"
                "X-E"
                "XQ"
                "FinePix S"
                "X-M"
                "FinePix F"
                "FinePix T"
                "FinePix HS"
                "FinePix Z"
                "FinePix A"
                "FinePix JZ"
                "FinePix JX"
                "FinePix JV"
                "FinePix J"
                "Instax" ] }

        let leica =
          { Name = "Leica"
            Ranges =
              [ "M-"
                "M10"
                "Q-"
                "TL"
                "Digilux"
                "V"
                "C"
                "D"
                "Q" ] }

        let nikon =
          { Name = "Nikon"
            Ranges =
              [ "D"
                "Z"
                "Coolpix B"
                "Coolpix W"
                "Coolpix A"
                "1 J"
                "1 S"
                "1 V"
                "Coolpix S"
                "Coolpix L"
                "Coolpix P" ] }

        let kodak =
          { Name = "Kodak"
            Ranges =
              [ "Pixpro Astro Zoom"
                "Pixpro AZ"
                "Pixpro S"
                "EasyShare C"
                "EasyShare M"
                "EasyShare Z"
                "EasyShare Touch"
                "EasyShare Sport"
                "EasyShare Mini"
                "EasyShare V"
                "EasyShare P"
                "DC"
                "DX"
                "DCS"
                "LS" ] }

        let panasonic =
          { Name = "Panasonic"
            Ranges =
                let dmcRanges =
                  [ "DMC-S"
                    "DMC-G"
                    "DMC-GX"
                    "DMC-GF"
                    "DMC-GH"
                    "DMC-GM"
                    "DMC-FZ"
                    "DMC-FP"
                    "DMC-FH"
                    "DMC-FS"
                    "DMC-FX"
                    "DMC-ZS"
                    "DMC-LX"
                    "DMC-LS"
                    "DMC-LZ"
                    "DMC-TS"
                    "DMC-TZ"
                    "DMC-FT"
                    "DMC-SZ"
                    "DMC-LC" ]
                let dcRanges =
                    dmcRanges
                    |> List.map (fun x -> x.Replace("DMC", "DC"))

                dmcRanges @ dcRanges }

        let samsung =
          { Name = "Samsung"
            Ranges =
              [ "NX"
                "WB"
                "Galaxy"
                "SH"
                "PL"
                "ST"
                "MV"
                "DV"
                "EX"
                "AQ"
                "HZ"
                "CL"
                "Digimax"
                "NV"
                "L"
                "SL"
                "GX"
                "i" ] }

        let sony =
          { Name = "Sony"
            Ranges =
              [ "Alpha"
                "a"
                "DSC-RX"
                "DSC-HX"
                "DSC-AS"
                "FDR-X"
                "SLT-A"
                "DSC-W"
                "DSC-WX"
                "DSC-QX"
                "NEX"
                "DSC-TX"
                "DSC-H"
                "DSC-T"
                "DSC-D"
                "DSC-F"
                "DSC-P"
                "DSC-S"
                "DSC-U"
                "DSC-G"
                "DSC-M"
                "DSC-N"
                "DSC-V" ] }

        let pentax =
          { Name = "Pentax"
            Ranges =
              [ "K"
                "KP"
                "K-S"
                "Q-S"
                "XG"
                "Q"
                "WG"
                "MX"
                "Optio LS"
                "Optio WG"
                "Optio VS"
                "Optio RZ"
                "Optio RS"
                "Optio S"
                "Optio I"
                "Optio H"
                "Optio E"
                "Optio A"
                "Optio M"
                "Optio W"
                "Optio T"
                "EI-" ] }

        let casio =
          { Name = "Casio"
            Ranges =
              [ "Exilim EX-ZR"
                "Exilim EX-TR"
                "Exilim EX-"
                "Exilim EX-ZS"
                "Exilim EX-H"
                "Exilim EX-S"
                "QV-"
                "GV-" ] }

        let hasselblad =
          { Name = "Hasselblad"
            Ranges =
              [ "X1D II 50C"
                "X1D" ] }

        let ricoh =
          { Name = "Ricoh"
            Ranges =
              [ "WG-"
                "G-"
                "GR"
                "GXR"
                "CX"
                "PX"
                "GXR P"
                "GX"
                "Caplio R"
                "Caplio GX"
                "RDC-" ] }

        let konicaMinolta =
          { Name = "Konica Minolta"
            Ranges =
              [ "DiMAGE Z"
                "DiMAGE X"
                "DiMAGE A"
                "DiMAGE G"
                "DiMAGE E"
                "DiMAGE F"
                "DiMAGE S"
                "Maxxium"
                "Dynax"
                "KD"
                "e-mini" ] }

        let kyocera =
          { Name = "Kyocera"
            Ranges =
              [ "Finecam SL"
                "Finecam M"
                "Finecam S"
                "Finecam L" ] }

        let sigma =
          { Name = "Sigma"
            Ranges =
              [ "DP"
                "Quattro"
                "Merrill"
                "SD" ] }

        let brandRanges =
          [ olympus
            canon
            fujifilm
            leica
            nikon
            kodak
            panasonic
            samsung
            sony
            pentax
            casio
            hasselblad
            ricoh
            konicaMinolta
            kyocera
            sigma ]

    [<AutoOpen>]
    module HelperFunctions =
        /// Separate model names for alterate names.
        /// 
        /// For example, `PowerShot S110 (Digital IXUS V / Example 2)` becomes
        /// `[| PowerShot S110 ; Digital IXUS V; Example 2 |]`.
        /// 
        /// Strongly assumes that text in parentheses separated by `" / "` are
        /// alternate names, which may not be the case (see
        /// `Galaxy Camera (Wi-Fi)`)
        let separateAltNames (modelName: string) =
            let removeClosingParen = modelName.Replace(")", "")
            let replaceOpeningParen = removeClosingParen.Replace ("(", "/ ")
            replaceOpeningParen.Split(" / ")

        /// Create regex that looks for the occurrences of each token in given
        /// sequence, such that a string with each token present in any position
        /// will match.
        /// 
        /// Example regex string: `^(?=.*\bOlympus\b)(?=.*\bAir\b).*$`
        let searchNameRegEx tokenizedModelName =
            [ for token in tokenizedModelName do
                  sprintf "(?=.*\\b%s\\b)" token ]
            |> String.concat ""
            |> sprintf "^%s.*$"

        /// Creates array of `Regex` object to match against array of alternate
        /// names for a camera model.
        let tokenizedModelNames (modelNames: string []) =
            modelNames
            |> Array.map (
                // split on space
                (fun x -> x.Split([| ' ' |]))
                >> searchNameRegEx
                >> (fun x -> Regex(x)))

        /// Returns all strings that match with at least on of the regexes in
        /// the given array.
        let searchModelNames (regexs: Regex []) (strsToSearch: string seq) =
            let filterFunc (str: string) =
                regexs
                |> Array.exists (fun rx -> rx.IsMatch(str))

            strsToSearch
            |> Seq.filter (filterFunc)

        /// Creates map from camera models, to other camera models that it would
        /// also match with (given it's regexes).
        /// 
        /// For example, "a7R" would match with all cameras that match with
        /// "a7R II" even though the latter models are not the former.
        let mapOfSubsetRegexes (regexs: (string * (Regex [])) list) =
            let modelNamesLower =
                regexs
                |> List.map (fst >> fun x -> x, x.ToLower())

            let regexMap =
                regexs
                |> Map.ofList

            [ for outerModelName, regex in Map.toSeq regexMap do
                let subsetModelNames =
                  [ for modelName, lowerCaseModelName in modelNamesLower do
                        if
                            // ignore if the same model
                            outerModelName <> modelName &&
                            // if the outerModelName's regex matches against
                            // modelName
                            regex |> Array.exists (fun x -> x.IsMatch(lowerCaseModelName))
                        then modelName ]
                outerModelName, subsetModelNames ]
            |> Map.ofList

        /// All camera listing IDs and associated dictionaries that possess of
        /// the given IDs.
        let dictsOfIds (cameraDicts: CamIdDictTyple []) (cameraIdsOfBrand: CameraId list) =
            let cameraIdSet = cameraIdsOfBrand |> Set.ofList

            cameraDicts
            |> Array.filter (fun (cameraId, _) -> Set.contains cameraId cameraIdSet)

        /// All camera model names that belong to a specific brand.
        let modelsOfBrand brand (dprSpecs: DpreviewSpec []) =
            dprSpecs
            |> Array.filter (fun sp -> sp.Manufacturer = brand)
            |> Array.map (fun sp -> sp.ModelName)
            |> List.ofArray

        /// Group cameras moodels that contain a specific camera range within
        /// the text. Append those cameras that did not match against any range
        /// in the list of ranges.
        let groupByRange (ranges: string list) (models: string list) =
            let ranges = 
              [ for range in ranges do
                    let rangeLower = range.ToLower()
                    let rangeIds =
                      [ for mn in models do
                            let mnLower = mn.ToLower()
                            if mnLower.Contains(rangeLower)
                            then mn ]
                    range, rangeIds ]

            let matchedModels = ranges |> List.collect snd |> Set.ofList
            let modelsSet = Set.difference (Set.ofList models) matchedModels

            ranges @ [ "Unmatched", modelsSet |> Set.toList ]

        /// All camera listing IDs that contain a model as a substring.
        let cameraDictsMatchingModel (cameraDicts: CamIdDictTyple []) (modelNames: string list) =
            let cameraFieldsCombined =
                cameraDicts
                |> Array.map (fun (camId, map) -> camId, map |> concatDictionaryFields)

            let matchedModels = 
                [ for model in modelNames do
                    let matchingModels =
                      [ for cameraId, cameraDict in cameraFieldsCombined do 
                            if cameraDict.Contains(model)
                            then cameraId ]

                    model, matchingModels ]

            let allIds = cameraDicts |> Array.map fst |> Set.ofArray
            let matchedIds = matchedModels |> List.collect snd |> Set.ofList
            let unmatchedIds = (allIds - matchedIds) |> Set.toList

            matchedModels @ [ "Unmatched", unmatchedIds ]

        /// Group all camera listing IDs by matching on a regex based on the
        /// model name in DPReview, grouped by the brand of model.
        let groupedModelsToDicts filepath =
            let dprSpecs =
                  filepath
                  |> DataRetrieval.deserialiseDpreviewSpecs

            let groups brand =
                dprSpecs
                |> modelsOfBrand brand.Name
                |> groupByRange brand.Ranges

            [ for brand in brandRanges do
                  let groupedRanges =
                      groups brand
                      |> List.map (fun (x, y) -> x, Array.ofList y)
                      |> toDictionary
                  brand.Name, groupedRanges ]
            |> toDictionary

        /// Find all camera listing IDs that match against a regex based on
        /// a camera model name.
        /// 
        /// Removes the case where a camera model matches for other models.
        let private matchModelNamesToCameraIds
                (modelNames: CameraModelName list)
                (pageTitles: (CameraId * string) []) =
            let modelRegexes =
                modelNames
                |> List.map (fun x ->
                    let nameRegexes =
                        x.ToLower()
                        |> separateAltNames
                        |> tokenizedModelNames
                    x, nameRegexes)

            let subsetMap =
                modelRegexes
                |> mapOfSubsetRegexes

            let matchingModelIds =
              [ for modelName, modelNameRegex in modelRegexes do
                    // printfn "searching for %s" modelName
                    let matchingIds =
                        pageTitles
                        |> Array.filter (fun (_, pageTitle) ->
                            modelNameRegex
                            |> Array.exists (fun rx -> rx.IsMatch(pageTitle)))
                        |> Array.map fst
                    if matchingIds |> Array.isEmpty |> not
                    then modelName, matchingIds |> Set.ofArray ]
                |> Map.ofList

            [ for modelName in modelNames do
                if matchingModelIds |> Map.containsKey modelName
                then
                    let modelSet =
                        matchingModelIds
                        |> Map.find modelName

                    let subsetModels =
                        subsetMap
                        |> Map.find modelName

                    let subsets =
                        matchingModelIds
                        |> Map.filter (fun key value ->
                            subsetModels
                            |> List.contains key)
                        |> Map.toSeq
                        |> Seq.map snd
                        |> Set.unionMany
                    
                    let modelSetMinimal = modelSet - subsets

                    if subsets |> Set.isEmpty |> not
                    then
                        printfn "%s %d" modelName (Set.count subsets)
                        printfn "%d" (Set.count modelSetMinimal)
                    
                    (modelName, modelSetMinimal |> Set.toArray) ]
            |> toDictionary

        /// Match camera brands and camera models in DPReview to cameras listing
        /// page titles in the given dataset.
        let private searchAndGroupGeneric dpreviewSpecsFilePath =
            let allFiles =
                DataRetrieval.allDicts()
            let dpreviewBrandCount =
                allFiles
                |> GroupBrands.countDPReviewBrands

            let searchAndGroupGivenBrand cameraBrand =
                printfn "\nsearching for models of %s" cameraBrand.Name

                let pageTitles =
                    dpreviewBrandCount
                    |> GroupBrands.cameraIdsGivenBrand cameraBrand.Name
                    |> dictsOfIds allFiles
                    |> Array.map (fun (cameraId, cameraDict) ->
                        cameraId, (cameraDict.["<page title>"] |> string).ToLower())

                let modelNames =
                    dpreviewSpecsFilePath
                    |> DataRetrieval.deserialiseDpreviewSpecs
                    |> modelsOfBrand cameraBrand.Name

                matchModelNamesToCameraIds modelNames pageTitles

            [ for brand in brandRanges do
                async {
                    let groupedBrands = searchAndGroupGivenBrand brand
                    printfn "completed searching for models of %s" brand.Name
                    return brand.Name, groupedBrands
                 } ]
            |> Async.Parallel
            |> Async.RunSynchronously
            |> Array.filter (fun (x, y) -> y.Count > 0)
            |> List.ofArray

        /// Search and group camera listing IDs based on camera brand and
        /// camera model.
        let searchAndGroupVerbose dpreviewSpecsFilePath =
            dpreviewSpecsFilePath
            |> searchAndGroupGeneric
            |> toDictionary

        /// Search and group camera list IDs into arrays of equivalent cameras.
        let searchAndGroupMinimal dpreviewSpecsFilePath =
            dpreviewSpecsFilePath
            |> searchAndGroupGeneric
            |> List.toArray
            |> Array.collect (snd >> (fun x -> x.Values |> Array.ofSeq))
