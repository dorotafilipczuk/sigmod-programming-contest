# JSON Files in Hyperion.ExampleCameraModels

Two most important source files are [GroupBrands.fs](GroupBrands.fs), used to separate cameras based on the camera manufacturer mentioned within the listing, and [SegmentProdRanges](SegmentProdRanges.fs), used both separate camera model names by the range and to separate listings by the same brand by whether the camera model name is matched within the listing page title.

[camera-ranges.json](/Hyperion.ExploreCameraModels/camera-ranges.json) - contains the camera models and ranges (based on rules decided by Oliver and Kojo) grouped by brand.

[groupedByBrand.json](/Hyperion.ExploreCameraModels/groupedByBrand.json) - contains the camera listing IDs that contain only one brand from DPReview (about 20,000 in total), no brand (about 10,000) and multi brands from DPReview.

[equivalent-camera-verbose-7.json](/Hyperion.ExploreCameraModels/equivalent-camera-verbose-7.json) - contains the camera listing IDs grouped first by brand and then by model. Unmatched models for a given brand possess the key `"Unmatched"`. In comparison to versio 4, now hyphens are optional so `Tough TG 5` and `Tough TG-5` are assumed to be the same camera.

[equivalent-cameras-7.json](/Hyperion.ExploreCameraModels/equivalent-cameras-7.json) - contains the camera listing IDs from above, but simply as an array of arrays, each inner array means that the cameras listings are for the same model.

See `/matchings` and `/matchings-detailed` for previous approaches taken, with the 4th in both cases assumed to be the most accurate.
