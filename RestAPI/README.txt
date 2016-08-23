This project was written with Visual Studio 2015 using Web API 2 and EntityFramework
* Note 1: I have not done much UI development for the last few years
* Note 2: The UI is simply to show that the required methods are implemented and NOT my idea of a good UI
* Note 3: I have never used knockout before and it caused me to take more time than I wanted to, but I wanted to give it a shot for
  something new.

1. Database creation
  a. Run RestAPI\RestAPI\SQL\RestAPI_DB_Create.sql 
  b. Run RestAPI\RestAPI\SQL\Schema_And_Data.sql 
    i. Note that the USE statement is present - USE [RestAPI]; this can be changed, obviously
  
2. Ensure that the web.config contains the appropriate connection strings for DefaultConnection and RestAPIConnection - pointing to the
database created in step 1.

3. When running the application, the index page contains the standard login screen that comes with Web API projects
  a. I have imported the users from the CSV into the RestAPI DB
  b. The login on this page "maps" AspNetUsers to the User records that I imported
  c. The following usernames exist in the AspNetUsers table and can be used to log in:
      ESmith@test.com
      JiSmith@test.com
      CJones@test.com
      BBrasky@test.com
      BBrown@test.com
      Harrison@test.com
      Smith@test.com
      MBill@test.com
      MJackson@test.com
      ncarmello57@yahoo.com
      JGonzales@test.com
      TUser2@test.com
      AWatson@test.com
  d. The password for each user is "Pass1234!"
  e. The Register form does work, but new users are not "mapped" to the imported users (yet)
4. When logged in as any of the above listed users, you will see that the full list of users is shown, as well as states, Alabama cities
  and a google map
5. If you change the selected state, the cities in that state will show up in a mutli select box (only Alabama, Alaska and Arizona
  have cities)
6. When you select a user, that user's visited cities and states appear below the selection. Also, the google map will show markers for
  each visited city (you will have to zoom out for Alaska).
7. You may view the visited cities/states of any selected user no matter which user account you use to log in. However, you must be 
  only the currently logged in user is able to add or delete visits
  
  Nick Carmello
  ncarmello57@yahoo.com

  

