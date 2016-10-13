###################################
##### F1 Web API Instructions #####
###################################

Drivers '/api/drivers'
- / 
- /{int:year}

Results '/api/results'
- /{int:year}/{string:country}

Scrape '/api/scrape'
- /seasons
- /drivers
- /standings
- /results

Seasons '/api/seasons'
- /
- /{int:year}
- /{string:country}

Standings '/api/standings'
- /drivers/{int:year}
- /drivers/{int:year}/{int:position}
- /constructor/{int:year}
- /constructor/{int:year}/{int:position}