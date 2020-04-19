module Pages.Home

open Fulma
open Fable.React
open Fable.React.Props


let sideImage imageUrl = 
    Image.image [] [ img [ Src imageUrl; Style [ Height "100vh"] ] ]

let view = 
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