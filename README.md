# twitter-streamer
This is a project use Twitter API to get stream data from the 1% sampled stream end point to generate various statistic.

# Making API calls
All API endpoint in this project are public. No authetication needed. 

# Twitter Stream API calls
In order to get twitter sampled stream, an API call is made when application start. Call twitter API endpoint require authenticate. Authentication to be acquired
from twitter developer site. 

After possessing an authentication token please update the appsettings with a valid authentication code.
Below show the settings needed to be changed. 
- Open appsettings.json file
- Look for "TwitterBearerToken" in "AppSettings" section, then replace "{Your Twitter Bearer Token}" with a valid Bearer Token. 

# Available API Endpoint for sample statistic
- /MessageTotal  - Return the total count of messages received
- /Top10HashTag  - Return the Top 10 HashTag found in the received messages

