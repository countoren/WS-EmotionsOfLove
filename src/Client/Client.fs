module Client

open Elmish.UrlParser
open Elmish
open Elmish.Navigation
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Shared


type Program = {
    url : string 
    navbarTitle : string 
    view : ReactElement
}

[<RequireQualifiedAccessAttribute>]
type Page = 
   | Home
   | About
   | Destiny
   | Sessions
   | ContactMe

let toPath =
    function
    | Page.Home -> "/"
    | Page.About -> "/about"
    | Page.Destiny -> "/destiny"
    | Page.Sessions -> "/session"
    | Page.ContactMe -> "/contactMe"

type Msg = Page

let pageParser =
    let map = UrlParser.map
    let s = UrlParser.s
    oneOf
        [ map Page.Home (s "")
          map Page.About (s "about")
          map Page.Destiny (s "destiny")
          map Page.Sessions (s "session")
          map Page.ContactMe (s "contactMe")
        ]
let urlParser location = parsePath pageParser location

type Model = { Page : Page }

/// The navigation logic of the application given a page identity parsed from the .../#info
/// information in the URL.
let urlUpdate (result:Page option) (model:Model) =
    match result with 
    | None ->  { model with Page = Page.Home } , Cmd.none
    | Some page ->  { model with Page = page } , Cmd.none


// defines the initial state and initial command (= side-effect) of the application
let init (p: Page option) =
    { Page = Page.Home }, Cmd.none

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg: Msg) (currentModel: Model): Model * Cmd<Msg> =
    { currentModel with Page = msg }, Navigation.newUrl (toPath msg)

let navbarBotton name dispatchFun = 
    Navbar.Item.a [ 
        Navbar.Item.Option.Props [ Style [ FontSize "larger" ]; OnClick dispatchFun ]
        ] [ str name ]

let sideImage imageUrl = 
    Image.image [] [ img [ Src imageUrl; Style [ Height "100vh"] ] ]

let aboutView = div [] [ str "About" ]
let homeView = 
           Tile.ancestor [ Tile.Option.Props [ Style [ 
               BackgroundImage "url(/images/background.jpg)"
               BackgroundSize "cover"  
              ] ] ]
              [ 
                Tile.parent [ Tile.Modifiers [ Modifier.IsHidden (Screen.Mobile, true)] ] [
                  Tile.child [] [ sideImage "/images/ManHead320.png" ]
                ]
                Tile.parent [ Tile.Size Tile.Is7; Tile.IsVertical] [
                  Tile.child [] []
                  Tile.child [ Tile.Option.Props [ Style [ PaddingLeft "7%"; PaddingRight "7%"]]] 
                      [ Box.box' [ Props [ Style
                        [ BackgroundColor "rgba(255, 255, 255, 0.61)"
                          FontSize "larger" 
                        ] ] ]
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

                Tile.parent [ ] [ Tile.child [ ] [ sideImage "/images/WomanHead320.png" ] ]
              ]

let view (model: Model) (dispatch: Msg -> unit) =
    div [ Style [ FontFamily "orpheus-pro" ] ]
        [ Navbar.navbar [ Navbar.Color IsWhite ]
           [ 
             Navbar.Item.div
               [ Navbar.Item.Option.Props
                   [ Style [ FontFamily "orpheus-pro"; FontSize "xx-large" ] ] 
               ] [ str "EmotionsOfLove" ]
             Navbar.Item.div [] []
             navbarBotton "Home" (fun _ -> dispatch Page.Home)
             navbarBotton "About" (fun _ -> dispatch Page.About)
             navbarBotton "Destiny" (fun _ -> dispatch Page.Destiny)
             navbarBotton "Sessions" (fun _ -> dispatch Page.Sessions)
             navbarBotton "Contact Me" (fun _ -> dispatch Page.ContactMe)
            ] ;
           div [] [ str ( toPath model.Page)];
           homeView
        ]
        

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
|> Program.toNavigable urlParser urlUpdate 
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
