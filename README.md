# backend_project

You can set up this application with this command: ```docker-compose up -d```

Examples for using this projects' API ( cURL commands )

Adding tag to Tags Repository:

	curl -X POST -H "Content-Type: application/json" -d "{\"Name\":\"Test Tag\"}" http://172.10.0.2/api/tags

Adding asset to Asset Repository:

	curl -X POST -H "Content-Type: application/json" -d "{\"Name\":\"Asset\",\"Properties\":{}}" http://172.10.0.2/api/assets
  
	
Adding SNMP Agent to Agent Repository:

	curl -X POST -H "Content-Type: application/json" -d "{\"AssetId\":{id},\"IpAddress\":\"172.10.0.4\",\"Community\":\"public\"}" http://172.10.0.2/api/agents/create
	

Binding tags to assets:

	curl -X GET  http://172.10.0.2/api/tags/{id}/assets/add?tagId={tagId
	

Display all information on assets:

	curl -X GET http://172.10.0.2/api/assets?loadRelated=true
	

Display information on given asset:

	curl -X GET http://172.10.0.2/api/assets/{id}?loadRelated=true
	

Make SNMP Poller Service query given agent:

	curl -X GET http://172.10.0.2/api/agents/{id}/poll
