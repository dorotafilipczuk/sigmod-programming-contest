namespace Hyperion.ExploreCameraNames

open System.Collections.Generic
open System.Text.RegularExpressions

open Hyperion.ExploreCameraNames.Utils

module GroupBrands =
    /// Count the number of times the brands in DPReview are mentioned in any
    /// value of the given dictionaries.
    let countDPReviewBrands (idDicts: CamIdDictTyple []) =
        [ for cameraId, map in idDicts do
            let values =
                map
                |> concatDictionaryFields
            let allBrandOccurances =
              [ for brand in dpreviewCameraBrands do
                    let count = Regex.Matches(values, brand).Count
                    (brand, count) ]
            cameraId, allBrandOccurances ]

    /// Filter all camera IDs to those mentioning no DPReview brand.
    let cameraIdsNoBrands (brandedIdDicts: (CameraId * (string * int) list) list) =
        brandedIdDicts
        |> List.filter (fun (_, brandOccurrances) ->
            brandOccurrances
            |> List.sumBy snd
            |> (fun sum -> sum = 0))

    /// Filter all camera IDs to those mentioning exactly one DPReview brand
    /// (any number of times).
    let cameraIdsOneBrand (brandedIdDicts: (CameraId * (string * int) list) list) =
        brandedIdDicts
        |> List.filter (fun (_, brandOccurrances) -> 
            brandOccurrances
            |> List.filter (snd >> (fun x -> x > 0))
            |> List.length
            |> (fun sum -> sum = 1))

    /// Filter all camera listing IDs to those mentioning a specific DPReview
    /// camera brand and no other DPReview camera brands.
    let cameraIdsGivenBrand brand (brandedIdDicts: (CameraId * (string * int) list) list) =
        brandedIdDicts
        |> List.filter (fun (_, brandOccurrances) -> 
            brandOccurrances
            |> List.filter (fun (br, x) -> x > 0 && br = brand)
            |> List.length
            |> (fun sum -> sum = 1))
        |> List.map fst

    /// Filter all camera listing IDs to those mentioning at least one
    /// DPReview camera brand.
    let cameraIdsMultiBrand (brandedIdDicts: (CameraId * (string * int) list) list) =
        brandedIdDicts
        |> List.filter (fun (_, brandOccurrances) -> 
            brandOccurrances
            |> List.filter (snd >> (fun x -> x > 0))
            |> List.length
            |> (fun sum -> sum > 1))

    /// Group all camera IDs by the one brand mentioned.
    let groupedCameraIdsOneBrand (allCameraIdsOneBrand: ((CameraId * (string * int) list) list)) =
        allCameraIdsOneBrand
        |> List.map (fun (cameraId, brandCounts) ->
            let (brand, count) =
                brandCounts
                |> List.find (fun (_, x) -> x > 0) 
            brand, cameraId)
        |> List.groupBy fst
        |> List.map (fun (brand, xs) ->
            let cameraIds =
                xs |> List.map snd |> List.toArray
            brand, cameraIds)

    /// Group all cameras with no brand in a singleton list.
    let groupedCameraIdsNoBrand (allCameraIdsNoBrand: ((CameraId * (string * int) list) list)) =
        let cameraIds =
            allCameraIdsNoBrand
            |> List.map fst
            |> List.toArray

        [ "No Brand", cameraIds ]

    /// Group all cameras with more than one brand, using the camera listing ID
    /// as a dictionary key and the array of brands mentioned, in a singleton
    /// list.
    let groupedCameraIdsMultiBrand (allCameraIdsMultiBrand: ((CameraId * (string * int) list) list)) =
        let cameraMultiBrands =
            allCameraIdsMultiBrand
            |> List.map (fun (cameraId, brandCounts) ->
                let multiBrand =
                    brandCounts
                    |> List.filter (fun (_, x) -> x >= 1)
                    |> List.map fst
                    |> List.toArray
                cameraId, multiBrand)
            |> List.toArray
            |> toDictionary

        [ "Multiple Brands", cameraMultiBrands ]

    /// Group all counts of camera brands filtered by no brands, one brand
    /// and multiple brands into one list.
    let groupCameraSpecsByBrand 
            (allCameraIdsOneBrand: ((CameraId * (string * int) list) list))
            (allCameraIdsNoBrand: ((CameraId * (string * int) list) list))
            (allCameraIdsMultiBrand: ((CameraId * (string * int) list) list)) =

        let oneBrandIds =
            groupedCameraIdsOneBrand allCameraIdsOneBrand
            |> List.map (fun (brand, xs) ->
                brand, xs |> Seq.cast<obj> :> obj)

        let noBrandIds =
            groupedCameraIdsNoBrand allCameraIdsNoBrand
            |> List.map (fun (brand, xs) ->
                brand, xs |> Seq.cast<obj> :> obj)

        let mutliBrandIds =
            groupedCameraIdsMultiBrand allCameraIdsMultiBrand
            |> List.map (fun (brand, idDict) -> brand, idDict :> obj)

        oneBrandIds @ noBrandIds @ mutliBrandIds
        |> toDictionary