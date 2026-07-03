shebang := if os() == 'windows' {
  'pwsh.exe'
} else {
  '/usr/bin/env pwsh'
}

set shell := ["pwsh", "-c"]

set windows-shell := ["pwsh.exe", "-NoLogo", "-Command"]

default:
    Write-Host "fraquent targets: init build test"

init:
    mise install
    pnpm install
    lefthook install

build:
    dotnet build Projects.slnx

test:
    dotnet test
