namespace Hyperion.ExploreCameraNames

open System.Collections.Generic

module BrandFieldStats =
    /// Count the number of uses of keys names among all dictionaries
    let groupKeys (dicts: Dictionary<SpecKey, obj> seq) =
        dicts
        |> Seq.collect (fun x -> x.Keys :> IEnumerable<string> )
        |> Seq.countBy id

    /// Print the number of occurrences of all keys names among all dictionaries.
    let getKeyCount (dicts: Dictionary<SpecKey, obj> seq) =
        dicts
        |> groupKeys
        |> Seq.sortByDescending snd
        |> Seq.take 100
        |> Seq.iter (fun (key, count) -> printfn "%s, %d" key count)

    /// Filter all the dictionaries for those with a "brand" key, returning the
    /// the camera listing ID and the value bound to the key converted to a
    /// string.
    let allBrandFields (idDicts: (CameraId * Dictionary<SpecKey,obj>) seq) =
        idDicts
        |> Seq.choose (fun (cameraId, x) -> 
            if x.ContainsKey("brand")
            then Some (cameraId, x.["brand"] |> string)
            else None)

    /// Count the number of occurrences of each value of the "brand" key.
    let countAllBrandNames (brandValues: (CameraId * string) seq) =
        brandValues
        |> Seq.countBy snd
        |> Seq.sortByDescending snd

    /// Return the camera listing IDs which have the "brand" key bound to an
    /// array.
    let allBrandsWithLists (brandValues: (CameraId * string) seq) =
        brandValues
        |> Seq.groupBy snd
        |> Seq.filter (fun (key, _) -> key.Contains("["))

    /// Return all the camera IDs and brand key values where the value ends with
    /// " Web Site".
    let allBrandsWithWebSiteSuffix (brandValues: (CameraId * string) seq) =
        brandValues
        |> Seq.filter (fun (_, x) -> x.Contains(" Web Site"))