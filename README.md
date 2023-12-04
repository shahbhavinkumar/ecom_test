Accepts two optional arguments: rating, and work key.
If no arguments are passed in, output the total number of works for each date present in the dataset.
If a rating is passed in, output the work key for each work with that rating, ordered by most recent first.
If a work key is passed in:
First look up the key in the dataset. If it has an edition key, use that instead. If the key is not found, return “Work not found”.
Save the following information about the work or edition to a file in JSON format: title, first subject, and author name.
Output the name of the saved file.


<div>
    <p>Read Me:</p>
    <p>
        This project is assuming that you can either have a Rating or Work Key or none. </p>
    <p> Also, please make sure the settings in appsettings in API project are correct. Currently its looking following vital configs</p>

    <p>DataURL : which is the searchFile downloaded from the dumps on the openlibrary</p>
    <p>EndPointURL : which is the https://openlibrary.org</p>
    <p>SaveJsonFilePath : which is the path where the json files are saved</p>
     
      <p>Currently, it's not looking at the edition when search for a book according to problem statement, but taking the first or default book, since there is no mention of which
          edition is to be selected etc. The project is broken down into 3 sub projects, one for Client, Other for API and Shared for POCO's. No Db was used since it did not seem to be a requirement.
      </p>

   <p>Few notable concepts to look for here are Virtualization in Blazor, Swagger for testing the API Endpoints, Caching on the API for search data</p>

</div>
