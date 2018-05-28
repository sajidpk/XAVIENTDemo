
# set AWS keys

aws configure set aws_access_key_id $OctopusParameters["AWS Account.AccessKey"]  --profile Prod
aws configure set aws_access_key_id $OctopusParameters["AWS Account.SecretKey"]   --profile Prod
aws configure set default.region us-west-2  --profile Prod


dotnet lambda package-ci --Profile Prod   -sb  serverlessstore -ot serverless.json -cfg  aws-lambda-tools-defaults.json -pcfg True
