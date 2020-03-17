import json

with open("output/submission.csv", 'w') as file_write:
    file_write.write("left_spec_id, right_spec_id\n")

    with open("dataset/matching_cameras.json", 'r') as file_read:
        data = json.load(file_read)
        for cluster in data:
            for i in cluster:
                for j in cluster:
                    if i != j:
                        file_write.write(i + "," + j + "\n")
