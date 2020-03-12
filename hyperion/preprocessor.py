import json


def preprocess_dataset(csv_files=None):
    if csv_files is None:
        csv_files = ["./datasets/sigmod_large_labelled_dataset.csv", "./datasets/sigmod_medium_labelled_dataset.csv"]

    groups = []
    unknowns = []

    for csv_file in csv_files:
        with open(csv_file, "r") as f:
            for i, line in enumerate(f):
                if i == 0:
                    continue
                left, right, label = line.strip().split(",")

                if label == "1":
                    flag = True
                    for group in groups:
                        if left in group and right in group:
                            flag = False
                            break
                        elif left in group and not (right in group):
                            group.add(right)
                            flag = False
                            break
                        elif right in group and not (left in group):
                            group.add(left)
                            flag = False
                            break
                    if flag:
                        groups.append({left, right})

                elif label == "0":
                    unknowns += [left, right]

    groups = [list(group) for group in groups]
    return groups, unknowns


def get_record_in_text(product_url):
    """
    Gets the json file of product_url
    :param product_url:
    :return:
    """

    folder_name, _, product_id = product_url.split("/")
    with open ("./datasets/2013_camera_specs/{}/{}.json".format(folder_name, product_id), "r") as f:
        return f.read()


if __name__ == '__main__':
    groups, unknowns = preprocess_dataset()
    print(json.dumps(groups, indent=4))

    print(get_record_in_text("www.ebay.com//45749"))

