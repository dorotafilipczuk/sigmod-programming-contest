""" This script converts "matching_cameras.json" file to the submission format.

The "dataset" directory currently contains only the "matching_cameras.json"
file, which reflects the "transitive closure". The script below converts that
file into the submission format. It produces a file "submission.csv" and saves
it to the "output" directory. The resulting list of pairs of camera IDs have a
symmetric property, i.e. if a pair a,b is in the list, then a pair b,a is also
included.

Author: dorotafilipczuk
"""

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
