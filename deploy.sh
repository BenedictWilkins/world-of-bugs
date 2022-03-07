REPO_NAME="world-of-bugs"

rm -rf ./config/$REPO_NAME
mkdir -p ./config/$REPO_NAME
cp ./config/_default/config.toml ./config/$REPO_NAME/config.toml

X='baseURL'
Y="baseURL = \"/$REPO_NAME\""
sed -i "/$X/c\\$Y" ./config/$REPO_NAME/config.toml 

hugo --environment world-of-bugs
git add .
git commit -m "updates"
git push