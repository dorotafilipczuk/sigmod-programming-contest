# sigmod-programming-contest

RULE-BASED SOLUTION

This branch currently stores the rule-based solution. In this approach, the 
entities are divided by brand and grouped by MPN. For details, see
"rule-based-solution.py".

This solution results in F-Measure of 0.23 (precision = 1, recall = 0.13).

TRANSITIVE CLOSURE SOLUTION (old)

The transitive closure solution is in the "old_solutions_backup" directory.

Inside there, the "dataset" directory contains only the "matching_cameras.json"
file. To  run the code that converts that file into the submission format, type:

python3 transitive_closure.py

into the terminal. This will produce a file "submission.csv" and save it to the
"output" directory. This "submission.csv" file is what we currently have
submitted.
