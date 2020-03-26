namespace Hyperion.ExploreCameraNames

open System.Collections.Generic
open System.IO
open System.Text.Json

type CameraBrandName = string
type CameraModelName = string

type DpreviewSpec =
  { Manufacturer: CameraBrandName
    ModelName: CameraModelName }

module DataRetrieval =
    /// All the directories of the websites.
    let websiteDirectoryFiles dirPath =
        Directory.GetFiles(dirPath)

    /// Convert (relative) path to a JSON camera listing into tuple of the
    /// listing ID number and path to the file.
    let getJsonId (path: string) =
        let pathSplit = path.Split("\\")

        match pathSplit with
        | [| _; _; _; filename |] ->
            let cameraId = filename.Split(".json").[0]
            (cameraId, path)
        | _ ->
            failwith "Relative path expected to be \"../2013_camera_specs/<website>/<listing_number>.json\""

    /// Creating camera listing ID from website and listing ID number.
    let cameraToId website cameraNumber: CameraId =
        sprintf "%s//%s" website cameraNumber

    /// Creating dictionary from path to listing.
    let deserialiseJsonToDictAsync filepath: Async<Dictionary<SpecKey, obj>> =
        async {
            use fs = File.OpenRead(filepath)
            let! cameraDict = 
                JsonSerializer.DeserializeAsync<Dictionary<string, obj>>(fs).AsTask()
                |> Async.AwaitTask
            return cameraDict
        }

    /// Read files within website directory of camera listings and deserialise
    /// into array of camera ID and dictionary of listing.
    let websiteToDicts (dirPath: string): CamIdDictTyple [] =
        let splitPath = dirPath.Split("\\", 3)
        let website = splitPath.[2]

        website |> printfn "deserialising files for %s"

        let cameraIds =
            dirPath
            |> websiteDirectoryFiles
            |> Array.map getJsonId
        
        [| for (cameraId, filepath) in cameraIds ->
            async {
                let! cameraDict = deserialiseJsonToDictAsync filepath
                return (cameraToId website cameraId, cameraDict)
            } |]
        |> Async.Parallel
        |> Async.RunSynchronously

    /// Deserialise all the listings for all websites.
    let allDicts(): CamIdDictTyple [] =
        let cameraSpecWebsiteDirectories =
            Directory.GetDirectories("..\\2013_camera_specs")

        let cameraSpecsDir =
            cameraSpecWebsiteDirectories
            |> Array.collect websiteToDicts

        cameraSpecsDir

    /// Read and deserialise DPReview camera specifications, returning
    /// each manufacturer and model name.
    let deserialiseDpreviewSpecs filepath =
        async {
            use fs = File.OpenRead(filepath)
            let! specs =
                JsonSerializer.DeserializeAsync<Dictionary<string, obj>[]>(fs).AsTask()
                |> Async.AwaitTask
            let specsSimplified =
                specs
                |> Array.map (fun map ->
                    { Manufacturer = map.["Manufacturer"] |> string
                      ModelName = map.["Model Name"] |> string })
            return specsSimplified
        }
        |> Async.RunSynchronously