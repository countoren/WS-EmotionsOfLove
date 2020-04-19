module Client

open Elmish.UrlParser
open Elmish
open Elmish.Navigation
open Elmish.React
open Fable.React
open Fable.React.Props
open Pages.Home
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Shared



[<RequireQualifiedAccessAttribute>]
type Page = 
   | Home 
   | About 
   | Destiny 
   | Sessions 
   | ContactMe

type PageInfo = {
    url : string 
    navbarTitle : string 
    view : ReactElement
}


let aboutView = 
    Tile.ancestor [ Tile.Option.Props [ Style [ BackgroundImage "url(/images/background.jpg)"; BackgroundSize "cover" ]]]
        [ 
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

                Tile.parent [ ] [ Tile.child [ ] [ sideImage "/images/river.png" ] ]
        ]

let pages = 
    dict [ 
        Page.Home, { url  = "";  navbarTitle = "Home";view = Pages.Home.view;  } 
        Page.About, { url  = "about"; navbarTitle = "About";view = aboutView } 
        Page.Destiny, { url  = "destiny"; navbarTitle = "Destiny";view = div [][ str "Destiny"] } 
        Page.Sessions, { url  = "sessions"; navbarTitle = "Session";view = div [][ str "Session"] } 
        Page.ContactMe, { url  = "contactMe"; navbarTitle = "Contact Me";view = div [][ str "Contact Me"] } 
    ]
    

type Msg = Page

let pageParser =
    let map = UrlParser.map
    let s = UrlParser.s
    pages 
    |> Seq.map (fun pageInfo -> map pageInfo.Key (s pageInfo.Value.url)) 
    |> Seq.toList |> oneOf

let urlParser location = parsePath pageParser location

type Model = { Page : Page; }

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
    { currentModel with Page = msg }, Navigation.newUrl ("/"+pages.Item(msg).url)

let navbarBotton name dispatchFun = 
    Navbar.Item.a [ 
        Navbar.Item.Option.Props [ Style [ FontSize "larger" ]; OnClick dispatchFun ]
        ] [ str name ]

let view (model: Model) (dispatch: Msg -> unit) =
    div [ Style [ FontFamily "orpheus-pro" ] ]
        [ Navbar.navbar [ Navbar.Color IsWhite ]
           ([ 
             Navbar.Item.div
               [ Navbar.Item.Option.Props
                   [ Style [ FontFamily "orpheus-pro"; FontSize "xx-large" ] ] 
               ] [ str "EmotionsOfLove" ]
             Navbar.Item.div [] []
            ] @ ( 
                pages 
                |> Seq.map (fun pageInfo-> navbarBotton pageInfo.Value.navbarTitle (fun _ -> dispatch pageInfo.Key) )
                |> Seq.toList
            ))

          pages.Item(model.Page).view


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
