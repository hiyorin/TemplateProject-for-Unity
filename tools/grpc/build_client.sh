# 出力先を作成
mkdir -p ../Assets/Scripts/Data/Entity

# コード生成
protoc -I protos --csharp_out ../Assets/Scripts/Data/Entity --grpc_out ../Assets/Scripts/Data/Entity protos/*.proto --plugin=protoc-gen-grpc=Grpc.Tools/tools/macosx_x64/grpc_csharp_plugin

# 確認
ls ../Assets/Data/Entity