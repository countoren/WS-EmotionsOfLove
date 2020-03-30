{ stdenv
, fetchurl
}:
  stdenv.mkDerivation rec {
    version = "3.1.102";
    netCoreVersion = "3.1.102";
    name = "dotnet-sdk-${version}";

# https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-aspnetcore-3.1.2-macos-x64-binaries
    src = fetchurl {
      url = "https://dotnetcli.azureedge.net/dotnet/Sdk/${version}/dotnet-sdk-${version}-osx-x64.tar.gz";
      # use sha512 from the download page
      sha512 = "00xs87zj94v6yr6xs294bficp6lxpghyfswhnwqfkx62jy80qr5fa2y7s10ich3cbm2daa8dby56iizdvi7rnlvp23gfkq12gq4w1g8";
    };


    unpackPhase = ''
      mkdir src
      cd src
      tar xvzf $src
    '';

    buildPhase = ''
      runHook preBuild
      echo -n "dotnet-sdk version: "
      ./dotnet --version
      runHook postBuild
    '';


    installPhase = ''
      runHook preInstall
      mkdir -p $out/bin
      cp -r ./ $out
      ln -s $out/dotnet $out/bin/dotnet
      runHook postInstall
    '';

    meta = with stdenv.lib; {
      homepage = https://dotnet.github.io/;
      description = ".NET Core SDK ${version} with .NET Core ${netCoreVersion}";
    };
  }
