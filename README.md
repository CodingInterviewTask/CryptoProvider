# CryptoProvider

## About the Application

This is a simple .NET app that uses the Binance API to show some info about several crypto pairs. Additionally, it includes a primitive authentication service and an unassuming UI on Angular.

To run the back-end part, you'll need to configure your IDE to start up both projects. To run the front-end part, you'll first need to run `npm install` and then start it with `ng serve`.

When you run the app, you'll see two buttons, a listbox, and an info card. Press "Log in." This will get you an access token to allow you to send a request to the API and retrieve information about various crypto pairs (you can now switch between them by picking pairs in the listbox.) Now Press "Live Mode." This will establish a WebSocket connection and update the data in the card every 5 seconds. This is how this app works in a nutshell.

The way it is written is purposefully unelaborate. And this is where your tasks come into action.

## Your Task

1. The lifespan of the access token is really short (15 seconds.) After expiration, you can no longer switch between the currency pairs. Don't change this limit, but rather implement functionality to refresh the access token whenever it expires.
2. The code base it pretty messy, as you may notice. Refactor the back-end projects as if it were a real business application. You don't need to go all out on it (like implementing request validations and so on), but we want you to tidy up the structure and clean up the most essential flaws. 

Additionally, pay attention to what other issues you'll find in this project. You don't have to fix them, but we'd like to hear what else you would change.
