module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Shared

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
type Model =
    { Counter: Counter option }

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
    | Increment
    | Decrement
    | InitialCountLoaded of Counter

let initialCounter() = Fetch.fetchAs<Counter> "/api/init"

// defines the initial state and initial command (= side-effect) of the application
let init(): Model * Cmd<Msg> =
    let initialModel = { Counter = None }
    let loadCountCmd = Cmd.OfPromise.perform initialCounter () InitialCountLoaded
    initialModel, loadCountCmd

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg: Msg) (currentModel: Model): Model * Cmd<Msg> =
    match currentModel.Counter, msg with
    | Some counter, Increment ->
        let nextModel = { currentModel with Counter = Some { Value = counter.Value + 1 } }
        nextModel, Cmd.none
    | Some counter, Decrement ->
        let nextModel = { currentModel with Counter = Some { Value = counter.Value - 1 } }
        nextModel, Cmd.none
    | _, InitialCountLoaded initialCount ->
        let nextModel = { Counter = Some initialCount }
        nextModel, Cmd.none
    | _ -> currentModel, Cmd.none


let safeComponents =
    let components =
        span []
            [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
                  [ str "SAFE  "
                    str Version.template ]
              str ", "
              a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
              str ", "
              a [ Href "http://fable.io" ] [ str "Fable" ]
              str ", "
              a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
              str ", "
              a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ] ]

    span []
        [ str "Version "
          strong [] [ str Version.app ]
          str " powered by: "
          components ]

let show =
    function
    | { Counter = Some counter } -> string counter.Value
    | { Counter = None } -> "Loading..."

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ] [ str txt ]

let navbarBotton name = 
    Navbar.Item.a [ Navbar.Item.Option.Props [ Style [ FontSize "larger" ]]] [ str name ]


let view (model: Model) (dispatch: Msg -> unit) =
    div [ Style [ FontFamily "orpheus-pro" ] ]
        [ Navbar.navbar [ Navbar.Color IsWhite ]
              [ 
                Navbar.Item.div
                  [ Navbar.Item.Option.Props
                      [ Style [ FontFamily "orpheus-pro"; FontSize "xx-large" ] ] 
                  ] [ str "EmotionsOfLove" ]
                Navbar.Item.div [] []
                navbarBotton "Home"
                navbarBotton "About"
                navbarBotton "Destiny"
                navbarBotton "Sessions"
                navbarBotton "Contact Me"
              ]

          Tile.ancestor [ Tile.Option.Props [ Style [ BackgroundImage "url(/images/background.jpg)"; BackgroundSize "cover" ] ] ]
              [ 
              Tile.parent [ Tile.Modifiers [ Modifier.IsHidden (Screen.Mobile, true)] ] [
                  Tile.child [] [ Image.image [] [ img [ Src "/images/ManHead320.png" ] ] ]
               ]
              Tile.parent [ Tile.Size Tile.Is7; Tile.IsVertical] [
                  Tile.child [] []
                  Tile.child [ Tile.Option.Props [ Style [ PaddingLeft "7%"; PaddingRight "7%"]]] 
                      [ Box.box' [ Props [ Style
                        [ BackgroundColor "rgba(255, 255, 255, 0.61)"
                          FontSize "larger" 
                        ] ] ]
                      ]
                      [ str """Hi, my name is Katie and I am an Emotion Code Practitioner.
                          I would love to help you release trapped emotions so you can make space for Love in your life.
                          """
                                br []
                                br []
                                str """
                          If you would like to learn more about this technique please click on About for detailed information. I look foward to hearing from you.
                          """ ]
                      ]
                  Tile.child [] []
              ]

              Tile.parent [ ] [
                  Tile.child [ ] [ Image.image [] [ img [ Src "/images/WomanHead320.png" ] ] ]
               ]
          ]
        ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
