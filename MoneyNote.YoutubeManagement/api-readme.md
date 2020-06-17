# Login
http://ulife.co/Login
## request:POST
				{
					"username":"uid",
					"password":"pwd"
				}
## response
				{
					"code": 0,
					"message": "Loged",
					"data": "L5YyGSGpd1BTDsugDtuUsVUUjUSMbt1/g8mEYDALoxAy3+OTiEnbBwVWzQa3o1lA0pECTwG7/hfZPHR6uymOgw=="
				}

# Category
http://ulife.co/Category/SelectAll
## request:POST
#### header["RequestSourceType"]="api"
#### header["Authorization"]="Bearer L5YyGSGpd1BTDsugDtuUsVUUjUSMbt1/g8mEYDALoxAy3+OTiEnbBwVWzQa3o1lA0pECTwG7/hfZPHR6uymOgw=="
				{
                    "title":"search by title""
				}
                // if dont want to search send empty object { } or title:""

## response 
                {
                    "code": 0,
                    "message": null,
                    "data": [
                        {
                            "title": "xxxx",
                            "id": "075ff2f1-ec33-435d-8442-fc63330b4f46",
                            "parentId": "9f5a5658-1359-4883-b2e8-1f76839a2de2",
                            "tenantId": "00000000-0000-0000-0000-000000000000",
                            "isDeleted": 0,
                            "createdBy": "00000000-0000-0000-0000-000000000000",
                            "createdAt": "2020-06-17T21:09:39",
                            "updatedBy": "00000000-0000-0000-0000-000000000000",
                            "updatedAt": null
                        }        
                    ]
                }

# Content
http://ulife.co/Content/SelectAll
## request:POST
#### header["RequestSourceType"]="api"
#### header["Authorization"]="Bearer L5YyGSGpd1BTDsugDtuUsVUUjUSMbt1/g8mEYDALoxAy3+OTiEnbBwVWzQa3o1lA0pECTwG7/hfZPHR6uymOgw=="
                {
	                "title":"",
	                "pageIndex":1,
	                "pageSize":1,
	                "parentId":"00000000-0000-0000-0000-000000000000",
	                "categoryIds":[]
                }
## response
                {
                    "code": 0,
                    "message": null,
                    "data": {
                        "data": [
                            {
                                "title": "xx",
                                "thumbnail": "zzzz",
                                "description": "xxyy",
                                "urlRef": "xxx",
                                "countView": 0,
                                "categoryIds": null,
                                "id": "c04cbdef-47e2-4829-a615-4add331593e9",
                                "parentId": "00000000-0000-0000-0000-000000000000",
                                "tenantId": "00000000-0000-0000-0000-000000000000",
                                "isDeleted": 0,
                                "createdBy": "00000000-0000-0000-0000-000000000000",
                                "createdAt": "2020-06-16T02:28:32",
                                "updatedBy": "00000000-0000-0000-0000-000000000000",
                                "updatedAt": null
                            }
                         ],
                        "itemsCount": 3,
                        "listCategory": [
                            {
                                "title": "xxxx 1",
                                "id": "8302a560-356f-4a9d-a65c-8b1e6bee4bb7",
                                "parentId": "9c125351-da2a-4a36-8fe9-57c6f5dc8542",
                                "tenantId": "00000000-0000-0000-0000-000000000000",
                                "isDeleted": 0,
                                "createdBy": "00000000-0000-0000-0000-000000000000",
                                "createdAt": "2020-06-15T23:16:52",
                                "updatedBy": "00000000-0000-0000-0000-000000000000",
                                "updatedAt": null
                            }],
                        "listRelation": [
                            {
                                "categoryId": "94b08df5-15d4-4856-bf60-3de3fb3d2766",
                                "contentId": "c04cbdef-47e2-4829-a615-4add331593e9",
                                "id": "10647225-d6c0-4593-b0a1-5ddaadc99d10",
                                "parentId": "00000000-0000-0000-0000-000000000000",
                                "tenantId": "00000000-0000-0000-0000-000000000000",
                                "isDeleted": 0,
                                "createdBy": "00000000-0000-0000-0000-000000000000",
                                "createdAt": "2020-06-16T02:33:41",
                                "updatedBy": "00000000-0000-0000-0000-000000000000",
                                "updatedAt": null
                            }]
                    }
                }
