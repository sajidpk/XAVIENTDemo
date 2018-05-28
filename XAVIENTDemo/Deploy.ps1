
aws configure set aws_access_key_id default_access_key

dotnet lambda package-ci --Profile Prod   -sb  serverlessstore -ot serverless.json -cfg  aws-lambda-tools-defaults.json -pcfg True
