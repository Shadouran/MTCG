**Design/Lessons learned**

The problem that hindered me that most was a problem with the PostgreSQL database.
PostgreSQL does not support Multiple Active Result Sets (MARS), so when executing a command using the ExecuteReader
method and trying to execute another query, it will throw an error saying another command is already active.
This was rather easily fixed by just using the Close method on the reader once I was finished with the result set.

Once I started using tasks this problem occured again, so I introduced semaphores in each repository classes.
I then realized rather quickly that this is obviously not going to work the way I intended it to, since these semaphores
still allow simultaneous queries with Postgre does not support.
For the solutions I created a seperate class that contains a single static semaphore which is now used by all repository classes
which guarantees that only one command is executed at a time.

The server is almost completely the same we developed in classes except it starts a new task for handling a client.
I adjusted the RouteParser a little to allow for GET parameters in the url to be parsed more or less fluently.
Apart from that I created new repositories, manager and route command but kept the overall design since there is literally no need
to change it.

The database is cleaned after every restart, to remove just comment out the the call to method ClearDatabase in Database.cs

**Unit Tests**

The unit tests I used are for the weakness chart and elo calculations since those impact the gameplay, apart from the cards themselves, the most.
I tested all standard cases of monster vs monster, spell vs spell, monster vs spell and all special interactions such as
goblin vs dragon or firelf vs dragon.
The elo calculation test cover all growth factors that get decided via games played/winrate.
Had I begun earlier I would have also added unit tests to the trading function since this is another critical part. Cards could easily get lost, duplicated or trades would not be accepted even though all requirements are fulfilled.

**Tracked Time**

- 15.01.2022 11:20 - 19:30 | user register/login, card packages
- 16.01.2022 11:00 - 15:00, 17:45 - 19:45 | stack, deck
- 17.01.2022 11:30 - 19:00 | routeParser GET parameters, task per client, user profile, stats
- 18.01.2022 11:25 - 18:50 | battle
- 19.01.2022 12:00 - 18:35 | trading
- 20.01.2022 11:50 - 15:00 | protocol, elo calculation tests, cleanup

**Github Link**

https://github.com/Shadouran/MTCG
