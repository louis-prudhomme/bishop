# Bishop

## Environment variables

Some environment variables are necessary to successfully run the program. They can be set in Visual Studio (Project →
Properties → Debug → Add).

ENV VAR | VALUE
--- | ---
COMMAND_SIGIL | `<any character, except «/»>`
DISCORD_TOKEN | `<discord token>`
MONGO_TOKEN | `<mongo token>`
MONGO_DB | `bishop_test`
WEATHER_API_KEY | `<weather api key>`
GOOGLE_CREDS | `<google credentials as json>`

## TODO


    // TODO use new count methods to replace use of CounterEntity objects in DB
    // TODO fallback on history counts instead of ConuterEntities
    // TODO merge history & counter by registering counters as ghost records
    // TODO old ghost records to new (with null instead of placeholder)