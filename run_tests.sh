#!/usr/bin/env bash
set -euo pipefail

echo "Running tests with coverage..." >&2
dotnet test --collect:"XPlat Code Coverage" --settings:"coverage.runsettings"

echo -e "\nCoverage artifacts generated under TestResults/*/coverage.*" >&2

if command -v reportgenerator >/dev/null 2>&1; then
  coverage_file=$(find . -path "*TestResults*coverage.opencover.xml" | head -n 1)
  if [ -z "$coverage_file" ]; then
    echo "No coverage.opencover.xml found under TestResults; skipping HTML generation." >&2
  else
    testresults_dir=$(dirname "$(dirname "$coverage_file")")
    target_dir="$testresults_dir/coveragereport"
    echo "Generating HTML report into ${target_dir}..." >&2
    reportgenerator \
      -reports:"${coverage_file}" \
      -targetdir:"${target_dir}" \
      -reporttypes:"Html"
    echo "HTML report ready at ${target_dir}/index.html" >&2
  fi
else
  echo "Tip: install reportgenerator to get HTML: dotnet tool install --global dotnet-reportgenerator-globaltool" >&2
fi
