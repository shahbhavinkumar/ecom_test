Accepts two optional arguments: rating, and work key.
If no arguments are passed in, output the total number of works for each date present in the dataset.
If a rating is passed in, output the work key for each work with that rating, ordered by most recent first.
If a work key is passed in:
First look up the key in the dataset. If it has an edition key, use that instead. If the key is not found, return “Work not found”.
Save the following information about the work or edition to a file in JSON format: title, first subject, and author name.
Output the name of the saved file.
