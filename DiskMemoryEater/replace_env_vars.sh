#!/bin/bash

template_file=$1
output_file=$2

# Read the template file line by line and replace placeholders with environment variable values
while IFS= read -r line; do
    while [[ $line =~ (\$\{([a-zA-Z_][a-zA-Z_0-9]*)\}) ]]; do
        var="${BASH_REMATCH[2]}"
        value="${!var}"
        line=${line//$BASH_REMATCH/$value}
    done
    echo "$line" >> "$output_file"
done < "$template_file"
