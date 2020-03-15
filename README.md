# Battlerite Bot is a ..
#### Fun / learning project
#### Personal use tool
#### Telegram bot
#### Discord bot (WIP)
#### Raspberry project
#### !Work in progress!

## Description
The Battlerite Bot is a Telegram (and soon Discord) bot that helps bringing 2-6 people together to play the game Battlerite and fit their schedules together. Battlerite is a team based arena game played either 2v2 or 3v3, either by entering matchmaking as a group of 2 or 3, or "in-house" by gathering 4 or 6 players in one lobby. 
One would message a command to the bot, telling which game mode(s) they're looking to play and at what time of day they're available. Once a party of 2 or 3 people (or 4, 5 or 6 for 2v2 in-house) are available at the same time, the bot sends a ping with player names and a time frame that's good for everyone (essentially the intersection of given time intervals).

For example: to say you're looking to play 2v2 from 18 to 20, you'd post "/go 2v2 18:00 - 20:00".

## Functional goals
A bot capable of handling the described use case, while being perceived useful and easy enough to use that my playgroup prefers the bot for match arranging over chatting.

24h uptime, easy deployment, ability to run on .net core on Raspberry Pi.

## Learning goals
* Learning how to create chat bots, and how to interract with Telegram and Discord.
* Automated deployment scripting (scripts not included).
* Scripting overall.
* Learning how to use Raspberry, especially through SSH.

  
## Outside of scope/focus
* Code quality. It's enough if I don't pull my hair when making changes.
