"""This file produces the rule-based solution.

print_versions() function is the one that groups items together.

generate_csv() turns the grouppings into the submission format (pairs).

compare() separates the correct matchings from the incorrect ones,
according to the labelled dataset.

create_extra_labelled_set() creates an extra labelled dataset:
"extra_labelled_data.csv".

Author: dorotafilipczuk
"""

from os import listdir
from os.path import isfile, isdir, join
import json

def get_wikipedia_brands():
    result = []
    with open("brand_names.json", "r") as file:
        brands = file.readlines()
        for name in brands:
            result.append(name.rstrip())
    return result

def print_versions():
    """A list of 40 active camera brands from Wikipedia."""
    wikipedia_brands = get_wikipedia_brands()

    """Clusters / sets of all cameras groupped by brand, model and version."""
    clusters = dict()
    for brand in wikipedia_brands:
        clusters[brand] = []
    clusters["Unknown"] = []

    path = './2013_camera_specs/'

    """ List all data sources only (directory names)."""
    onlyfolders = [f for f in listdir(path) if isdir(join(path, f))]

    for folder in onlyfolders:
        """List JSON filenames only."""
        onlyfiles = [f for f in listdir(join(path, folder)) if isfile(join(join(path, folder), f))]

        for filename in onlyfiles:
            with open(join(path, folder, filename),'r') as file:
                product = json.load(file)
                product["dorota-id"] = folder + "//" + filename.split(".")[0]
                product_str = json.dumps(product).lower()

                brand_found = False
                for brand in wikipedia_brands:
                    if brand.lower() in product_str:
                        clusters[brand].append(product)
                        brand_found = True

                if brand_found == False:
                    clusters["Unknown"].append(product)

    matchings = dict()

    keys = ["mpn",
            "product number mpn",
            "manufacturer part number",
            "manufacturer's part number",
            "part number",
            ]

    for brand in clusters:
        matchings[brand] = dict()
        matchings[brand]["Unknown"] = []
        for item in clusters[brand]:
            key_found = False

            for key in keys:
                if key_found == False and key in item:
                    key_found = True
                    if str(item[key]) not in matchings[brand]:
                        matchings[brand][str(item[key])] = []
                    matchings[brand][str(item[key])].append(item)

            if key_found == False:
                matchings[brand]["Unknown"].append(item)

    """Select the important sets (the sets that actually contain any matchings,
    i.e. more than 1 item).
    """
    important_matchings = dict()

    for brand in matchings:
        print(brand)
        important_matchings[brand] = dict()

        for model in matchings[brand]:
            if model != "Unknown" and len(matchings[brand][model]) > 1:
                important_matchings[brand][model] = matchings[brand][model]

        with open(join("./important_matchings", brand + ".json"), "w") as file:
            file.write(json.dumps(important_matchings[brand]))

        """Put the unknown models in a separate folder."""
        with open(join("./unknown_models", brand + ".json"), "w") as file:
            file.write(json.dumps(matchings[brand]["Unknown"]))

    print("Done!")

def generate_csv():
    submissions = open("all_submissions.csv", "w")
    submissions.write("left_spec_id,right_spec_id\n")

    path = './important_matchings'

    onlyfiles = [f for f in listdir(path) if isfile(join(path, f))]

    for file in onlyfiles:
        with open(join(path, file), "r") as json_file:
            brand = json.load(json_file)

            for version in brand:
                for i in brand[version]:
                    for j in brand[version]:
                        if i["dorota-id"] != j["dorota-id"]:
                            submissions.write(i["dorota-id"] + "," + j["dorota-id"] + "\n")
    submissions.close()

    def compare():
        labelled_dataset_filename = "large_labelled_data.csv"
        matchings = dict()

        with open(labelled_dataset_filename,'r') as file:
            lines = file.readlines()

            i = 1
            while i < len(lines):
                item1 = lines[i].split(",")[0]
                item2 = lines[i].split(",")[1]
                result = lines[i].split(",")[2].rstrip()

                if item1 not in matchings:
                    matchings[item1] = dict()
                matchings[item1][item2] = result

                if item2 not in matchings:
                    matchings[item2] = dict()
                matchings[item2][item1] = result
                i = i + 1
        print("Part 1 Done!")

        good = open("good.csv", "w")
        bad = open("bad.csv", "w")

        with open("all_submissions.csv", "r") as csv_file:
            lines = csv_file.readlines()

            i = 1
            while i < len(lines):
                item1 = lines[i].split(",")[0]
                item2 = lines[i].split(",")[1].rstrip()

                if item1 in matchings and item2 in matchings[item1]:
                    #print(item1)
                    if matchings[item1][item2] == '1':
                        good.write(lines[i])
                    else:
                        bad.write(lines[i])

                i = i + 1

        good.close()
        bad.close()
        print("Part 2 Done!")

def create_extra_labelled_set():
    submission_lines = ""
    with open("all_submissions.csv", "r") as submissions_file:
        submission_lines = submissions_file.readlines()

    with open("bad.csv", "r") as bad_file:
        bad_lines = bad_file.readlines()

        for line in bad_lines:
            submission_lines.remove(line)

    submission_lines[0] = submission_lines[0].rstrip() + ",label\n"
    i = 1
    while i < len(submission_lines):
        submission_lines[i] = submission_lines[i].rstrip() + ",1\n"
        i = i + 1

    with open("extra_labelled_data.csv", "w") as labelled_file:
        for line in submission_lines:
            labelled_file.write(line)

    print("Done!")


print_versions()
generate_csv()
compare()
create_extra_labelled_set()
