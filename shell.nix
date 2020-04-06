with import <nixpkgs>{};

mkShell {
  buildInputs = [ 
    figlet 
    nodejs
    fsharp
    yarn
    yarn2nix
    (callPackage (import ./dotnet.nix) {})
    (callPackage (import ./fake-darwin.nix) {})
    ( vscodeEnv { 
      mutableExtensionsFile = ./extension.nix; 
      nixExtensions = [ ];
      settings = {
        "FSharp.smartIndent" = true;
      };

    })
    
  ];


  shellHook = ''
    figlet 'safe app'
    echo 'safe quickstart: https://safe-stack.github.io/docs/quickstart/#create-your-first-safe-app'
    echo 'to start: fake build --target run'
  '';
}
