# TvMaze Scraper

A service that provides the cast of all the tv shows in the
TVMaze database, so we can enrich our metadata system with this information. The TVMaze
database provides a public REST API that you can query for this data.

## Prerequisites

* Visual studio 2019
* .Net core 3.1
* Sql Server

## Swagger UI

You can access the swagger ui generated docs on https://localhost:44351/swagger

## Technical comments:

* ShowsScrape end-point for getting data from TVMaze database and save it, then Shows end-point to get shows from db.
* I delete and create the Db again in the start-up to assume that db is empty when I scrape new data.
* I only scrape one page now for simplicity, real-life scenario we should have a job running with a queue mechanism.
* I used InMemoryDb for database unit tests.
* In a real-life scenario, integration tests must be written.
