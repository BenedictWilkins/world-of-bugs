#/bin/bash
# Clones any git repositories in the repos.txt file (repos must be https) using a git oauth token.

file="repos.txt"
REPOS=$(<$file)

ssh -tt ubuntu@$1 << EOF
    ROOT="\${HOME}/repos"
    mkdir -p "\$ROOT"
    conda activate PhD
    echo "$REPOS" |
    while IFS= read -r repo
    do 
        echo "INSTALLING: \$repo"
        cd "\$ROOT"
        git clone "$repo"
        name=\$(echo "\$repo" | rev | cut -d'/' -f 1 | rev | cut -d'.' -f 1)
        cd \$name
        pip install -e .
    done
EOF

