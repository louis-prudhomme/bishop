# Bishop

This is bot for a group of friends. It holds no pretention other than being fun and serve as a lab to experiment with dotnet, C#, Discord API, Github Actions and everything we do or use to build this little piece of software.

## Transcrypt

[Transcrypt](https://github.com/elasticdog/transcrypt) is used to encrypt several sensitive files across the project.

Its password can be found somewhere.

## Environment variables

Some environment variables are necessary to successfully run the program. 

They can be set in Visual Studio (`Project → Properties → Debug → Add`) as well as in Jetbrains Ryder (`Edit configuration → Environment variables`).

| ENV VAR         | VALUE                         |
|-----------------|-------------------------------|
| COMMAND_SIGIL   | `<any character, except «/»>` |
| DISCORD_TOKEN   | `<discord token>`             |
| MONGO_TOKEN     | `<mongo token>`               |
| MONGO_DB        | `bishop_test`                 |
| WEATHER_API_KEY | `<weather api key>`           |
| GRIVE_PATH      | `<./Resources/grive>`         |
