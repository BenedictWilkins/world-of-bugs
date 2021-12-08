ssh -tt ubuntu@$1 << EOF
    jupyter notebook --no-browser --NotebookApp.token="" & 
EOF

ssh -NL 8888:localhost:8888 ubuntu@$1
