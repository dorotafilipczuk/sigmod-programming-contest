namespace Hyperion.ExploreCameraNames

open System.Collections.Generic
open System.IO
open System.Text.Json

type CameraId = string
type SpecKey = string
type CamIdDictTyple = (CameraId * Dictionary<SpecKey, obj>)

module Utils =

    /// Convert sequence of pairs into <c>Dictionary</c>.
    let toDictionary (tuples: seq<'a * 'b>) =
        tuples
        |> dict
        |> (fun x -> new Dictionary<'a, 'b>(x))

    /// Print array of tuples, comma separated
    let printTuples xs =
        for x, y in xs do
            printfn "%A, %A" x y

    /// Brands used within the dataset and present on DPReview.
    let dpreviewCameraBrands =
        [
            "Agfa"
            "Fujifilm"
            "Leica"
            "Ricoh"
            "Canon"
            "Hasselblad"
            "Nikon"
            "Samsung"
            "Casio"
            "Kodak"
            "Olympus"
            "Sigma"
            "Contax"
            "Konica Minolta"
            "Panasonic"
            "Sony"
            "DxO"
            "Kyocera"
            "Pentax"
            "Lytro"
            "SeaLife"
            "Sigma"
            // "Zeiss"
        ]

    /// Collect and concatenate fields of a dictionary as a string.
    let concatDictionaryFields (dictionary: Dictionary<SpecKey, obj>) =
        dictionary.Values
        |> Seq.map (string)
        |> String.concat " "

    /// Serialise an object as JSON for a given filepath.
    let serialiseData filepath (data: 'a) =
        async {
            use fs = File.Create(filepath)
            do! JsonSerializer.SerializeAsync(fs, data) |> Async.AwaitTask
        }
        |> Async.RunSynchronously
