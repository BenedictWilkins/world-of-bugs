# run jupyter in the background on the server
ssh ubuntu@$1 "jupyter notebook --no-browser --NotebookApp.token='' & && exit"
ssh -NL 8888:localhost:8888 ubuntu@$1
