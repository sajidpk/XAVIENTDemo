﻿
# set AWS keys

aws configure set aws_access_key_id $OctopusParameters["AWS Account.AccessKey"]  --profile Prod
aws configure set aws_secret_access_key $OctopusParameters["AWS Account.SecretKey"]   --profile Prod
aws configure set default.region us-west-2  --profile Prod

# Bulid the package, and  store the package to S3  , this will update the   serverless.Json with new S3 path.

dotnet lambda package-ci --Profile Prod  -sb  serverlessstore -ot serverless.json 

#aws cloudformation delete-stack --stack-name serverlessdemo

# deploy Lambda with api gate way
aws cloudformation deploy --template serverless.json --stack-name serverlessdemo  --parameter-overrides BucketName=serverlessstore ShouldCreateBucket=false --capabilities CAPABILITY_IAM