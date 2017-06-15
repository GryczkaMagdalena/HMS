define({ "api": [
  {
    "type": "post",
    "url": "/Auth/AddToRole",
    "title": "AddToRole",
    "version": "0.1.1",
    "name": "AddToRole",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "UserID",
            "description": "<p>ID of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleName",
            "description": "<p>Name of role</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Status of successful response</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"User added to role Customer\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoleNotFound",
            "description": "<p>Role name is invalid.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserError",
            "description": "<p>User cannot be added to role.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserNotExists",
            "description": "<p>User cannot be found.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n \"status\":\"Role does not exists\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n     \"status\":User cannot be added to role Customer\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n     \"status\":User does not exists\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/CheckIn",
    "title": "CheckIn",
    "version": "0.1.4",
    "name": "CheckIn",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "emailOrLogin",
            "description": "<p>Email or login of user to be checked in</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roomNumber",
            "description": "<p>Number of room to which user will be checked in</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>User succesfully checked in</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n    {\n    \"status\":\"checkedIn\"\n    }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidRole",
            "description": "<p>Only user with role Customer can occupy room</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserAlreadyCheckedIn",
            "description": "<p>One customer can occupy only one room at the same time</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoomAlreadyOccupied",
            "description": "<p>If specified room is already occupied, validation fails</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>If Room or User cannot be found</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "TransactionFailed",
            "description": "<p>Some data on server cannot be requested, try again later or contact with Administrator</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{ \n    \"status\":\"invalidRole\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n   \"status\":\"userAlreadyCheckedIn\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n   \"status\":\"roomAlreadyOccupied\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{ \n   \"status\":\"invalidInput\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"transactionFailed\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/CheckOut",
    "title": "CheckOut",
    "version": "0.1.4",
    "name": "CheckOut",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "emailOrLogin",
            "description": "<p>Email or login of user to be checked out</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roomNumber",
            "description": "<p>For verification - number of room that is occupied by requested user</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>User succesfully checked out</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n    {\n    \"status\":\"checkedOut\"\n    }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidRole",
            "description": "<p>Only user with role Customer can occupy room</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserAlreadyCheckedOut",
            "description": "<p>If user does not occupy specified room, validation fails</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoomNotOccupied",
            "description": "<p>If specified room is not occupied, validation fails</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>If Room or User cannot be found</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "TransactionFailed",
            "description": "<p>Some data cannot be requested by server, try again later or contact administrator</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{ \n    \"status\":\"invalidRole\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n   \"status\":\"userAlreadyCheckedOut\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n   \"status\":\"roomNotOccupied\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{ \n   \"status\":\"invalidInput\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n   \"status\":\"transactionFailed\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/GetUserRoles",
    "title": "GetUserID",
    "version": "0.1.3",
    "name": "GetUserID",
    "group": "Auth",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "GUID",
            "optional": false,
            "field": "id",
            "description": "<p>ID of currently logged user</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response",
          "content": "HTTP/1.1 200 OK\n  {\n    \"id\":\"some29299guid\"\n  }",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/GetUserRoles",
    "title": "GetUserRoles",
    "version": "0.1.1",
    "name": "GetUserRoles",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "UserID",
            "description": "<p>ID of user</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "List",
            "optional": false,
            "field": "roles",
            "description": "<p>Roles of user</p>"
          }
        ]
      }
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserNotExists",
            "description": "<p>User cannot be found.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n     \"status\":User does not exists\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/Register",
    "title": "Register",
    "version": "0.1.4",
    "name": "Register",
    "group": "Auth",
    "description": "<p>This method can take all parameters, however WorkerType field will only be used for Worker role</p>",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "login",
            "description": "<p>Login of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "password",
            "description": "<p>Password of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "firstName",
            "description": "<p>Optional user name</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "lastName",
            "description": "<p>Optional user surname</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "phoneNumber",
            "description": "<p>Optional contact number</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleName",
            "description": "<p>One of roles (Worker,Customer,Manager,Administrator)</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "email",
            "description": "<p>Required user identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "workerType",
            "description": "<p>One of available types (Cleaner,Technician,None)</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>User succesfully created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n    {\n    \"status\":\"registered\"\n    }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of entries is not valid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "EmailNotUnique",
            "description": "<p>Email address have to be unique</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoleNameInvalid",
            "description": "<p>Role Name must be valid with existing roles</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "TransactionFailed",
            "description": "<p>Internal Server Error</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{ \n    \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{ \n   \"status\":\"userIdentifierNotUnique\"\n   }",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n   \"status\":\"roleNameInvalid\"\n   }",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n   \"status\":\"transactionFailed\"\n   }",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/RemoveFromRole",
    "title": "RemoveFromRole",
    "version": "0.1.3",
    "name": "RemoveFromRole",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "UserID",
            "description": "<p>ID of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleName",
            "description": "<p>Name of role</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Status of successful response</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"User removed from role Customer\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoleNotFound",
            "description": "<p>Role name is invalid.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserError",
            "description": "<p>User cannot be added to role.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserNotExists",
            "description": "<p>User cannot be found.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n \"status\":\"Role does not exists\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n     \"status\":User cannot be removed from role Customer\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n     \"status\":User does not exists\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/Token",
    "title": "Token",
    "version": "0.1.5",
    "name": "TokenLogin",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Login",
            "description": "<p>Email or login of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Password",
            "description": "<p>User's password</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "firstName",
            "description": "<p>First name of user.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "lastName",
            "description": "<p>Last name of user.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "email",
            "description": "<p>Optional email address of user</p>"
          },
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "role",
            "description": "<p>All roles that particular user have</p>"
          },
          {
            "group": "Success 200",
            "type": "Token",
            "optional": false,
            "field": "token",
            "description": "<p>Authentication token that should be send in every response as header (headerKey:Authenticate, headerValue: &quot;bearer &quot; + token)</p>"
          },
          {
            "group": "Success 200",
            "type": "DateTime",
            "optional": false,
            "field": "expiration",
            "description": "<p>Date when token expires</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n\"user\":{\n     \"firstName\":\"Abraham\",\n     \"lastName\":\"Lincoln\",\n     \"email\":\"president@usa.pl\",\n     \"role\":[\"Worker\"]\n},\n\"token\":\"blablabla121212\",\n\"expiration\":\"2017-05-07T20:49:48Z\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "LoginFailed",
            "description": "<p>Password is invalid.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InternalError",
            "description": "<p>This User does not exist.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n \"status\":\"failedToLogin\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 BadRequest\n{\n    \"status\":\"failed\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "optional": false,
            "field": "varname1",
            "description": "<p>No type.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "varname2",
            "description": "<p>With type.</p>"
          }
        ]
      }
    },
    "type": "",
    "url": "",
    "version": "0.0.0",
    "filename": "HotelManagementSystem/wwwroot/doc/main.js",
    "group": "C__Users_tirur_Source_Repos_HotelManagementSystem_HotelManagementSystem_HotelManagementSystem_wwwroot_doc_main_js",
    "groupTitle": "C__Users_tirur_Source_Repos_HotelManagementSystem_HotelManagementSystem_HotelManagementSystem_wwwroot_doc_main_js",
    "name": ""
  },
  {
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "optional": false,
            "field": "varname1",
            "description": "<p>No type.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "varname2",
            "description": "<p>With type.</p>"
          }
        ]
      }
    },
    "type": "",
    "url": "",
    "version": "0.0.0",
    "filename": "HotelManagementSystem/wwwroot/doc/ping/main.js",
    "group": "C__Users_tirur_Source_Repos_HotelManagementSystem_HotelManagementSystem_HotelManagementSystem_wwwroot_doc_ping_main_js",
    "groupTitle": "C__Users_tirur_Source_Repos_HotelManagementSystem_HotelManagementSystem_HotelManagementSystem_wwwroot_doc_ping_main_js",
    "name": ""
  },
  {
    "type": "post",
    "url": "/Case",
    "title": "Create",
    "version": "0.1.0",
    "name": "Create",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Case title</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Case details</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>Type of case - one of (0-Cleaner,1-Technician,2-None)</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "EstimatedTime",
            "description": "<p>Time in format &quot;HH:mm:ss&quot;</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Case was created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"created\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "delete",
    "url": "/Case/CaseID",
    "title": "Delete",
    "version": "0.1.0",
    "name": "Delete",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "CaseID",
            "description": "<p>Case identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Case was deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"removed\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Case with specified ID was not found</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "get",
    "url": "/Case",
    "title": "List",
    "version": "0.1.0",
    "name": "List",
    "group": "Case",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "cases",
            "description": "<p>List of all existing cases</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n  [\n      {\n      \"caseID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"title\":\"ExampleCase\",\n      \"description\":\"Clean something\",\n      \"workerType\":\"Technician\",\n      \"estimatedTime\":\"01:00:00\"\n      }\n  ]",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "get",
    "url": "/Case/CaseID",
    "title": "Read",
    "version": "0.1.0",
    "name": "Read",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "CaseID",
            "description": "<p>Case identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "caseID",
            "description": "<p>Case identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "title",
            "description": "<p>Case title</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "description",
            "description": "<p>Case details</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "workerType",
            "description": "<p>Worker type associated with this case</p>"
          },
          {
            "group": "Success 200",
            "type": "TimeSpan",
            "optional": false,
            "field": "estimatedTime",
            "description": "<p>Estimated time needed to perform task with this case</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"caseID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"title\":\"ExampleCase\",\n      \"description\":\"Clean something\",\n      \"workerType\":\"Technician\",\n      \"estimatedTime\":\"01:00:00\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of cases</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "get",
    "url": "/Case/Filter/WorkerType",
    "title": "ReadFilter",
    "version": "0.1.2",
    "name": "ReadFilter",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>(Cleaner or Technician)</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "caseID",
            "description": "<p>Case identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "title",
            "description": "<p>Case title</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "description",
            "description": "<p>Case details</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "workerType",
            "description": "<p>Worker type associated with this case</p>"
          },
          {
            "group": "Success 200",
            "type": "TimeSpan",
            "optional": false,
            "field": "estimatedTime",
            "description": "<p>Estimated time of case</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"caseID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"title\":\"ExampleCase\",\n      \"description\":\"Clean something\",\n      \"workerType\":\"Technician\",\n       \"estimatedTime\":\"01:00:00\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of cases</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "put",
    "url": "/Case?CaseID",
    "title": "Update",
    "version": "0.1.0",
    "name": "Update",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "CaseID",
            "description": "<p>Case identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Case title</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Case details</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>Type of case - one of (0-Cleaner,1-Technician,2-None)</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "EstimatedTime",
            "description": "<p>Time in format &quot;HH:mm:ss&quot;</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Case was updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"updated\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Case with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "post",
    "url": "/Home",
    "title": "Create",
    "version": "0.1.0",
    "name": "Create",
    "group": "Home",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Rule title</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Rule details</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Rule was created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"created\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of rules</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "delete",
    "url": "/Home?RuleID",
    "title": "Delete",
    "version": "0.1.0",
    "name": "Delete",
    "group": "Home",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RuleID",
            "description": "<p>Rule identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Rule was deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"removed\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Rule with specified ID was not found</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRequest",
            "description": "<p>Given ID does not appeal to any of rules</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "get",
    "url": "/Home",
    "title": "List",
    "version": "0.1.0",
    "name": "List",
    "group": "Home",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "rules",
            "description": "<p>List of all rules</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n   { \n   \"ruleID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n   \"name\":\"Rule1\",\n   \"description\":\"Rule 1 desc\"\n   }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "get",
    "url": "/Home?RuleID",
    "title": "Read",
    "version": "0.1.0",
    "name": "Read",
    "group": "Home",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RuleID",
            "description": "<p>Rule identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "ruleID",
            "description": "<p>Rule identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "title",
            "description": "<p>Rule title</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "description",
            "description": "<p>Rule details</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"ruleID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"title\":\"ExampleRule\",\n      \"description\":\"Restrict something\",\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of rules</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "put",
    "url": "/Home?RuleID",
    "title": "Update",
    "version": "0.1.0",
    "name": "Update",
    "group": "Home",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RuleID",
            "description": "<p>Rule identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Rule title</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Rule details</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Rule was updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"updated\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Rule with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "post",
    "url": "/Room",
    "title": "Create",
    "version": "0.1.0",
    "name": "Create",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "Number",
            "description": "<p>Number of room</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Room was created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"created\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Referenced ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "delete",
    "url": "/Room?RoomID",
    "title": "Delete",
    "version": "0.1.0",
    "name": "Delete",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Room identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Room was deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"removed\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Room with specified ID was not found</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidData",
            "description": "<p>Reference or object was not correct</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoomOccupied",
            "description": "<p>Room is occupied, cannot be deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"Room occupied, check user out before deletion\",\n             \"userID\":\"guidOfCheckedUser\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "get",
    "url": "/Room/GetClient/{RoomID}",
    "title": "GetClient",
    "version": "0.1.0",
    "name": "GetClient",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>ID of room</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "GUID",
            "optional": false,
            "field": "userID",
            "description": "<p>Id of client</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "firstName",
            "description": "<p>First Name of client</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "lastName",
            "description": "<p>Last Name of client</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  userID = \"someGuid\",\n  firstName = \"Guy\",\n  lastName = \"Fawkes\",\n  email = \"guest@hms.com\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRole",
            "description": "<p>Only user with role Administrator can access this method</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoomNotOccpied",
            "description": "<p>Room is not occupied by any customer</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoomNotFound",
            "description": "<p>Given ID not found in repository</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 403 Unauthorized",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"roomNotOccupied\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "get",
    "url": "/Room/RoomNumber",
    "title": "GetRoomNumber",
    "version": "0.1.0",
    "name": "GetRoomNumber",
    "group": "Room",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "roomNumber",
            "description": "<p>Number of room</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"roomNumber\":\"1\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRole",
            "description": "<p>Only user with role Customer can access this method</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserNotCheckedIn",
            "description": "<p>Current user is not checked in any room</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 403 Unauthorized",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"userNotCheckedIn\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "get",
    "url": "/Room",
    "title": "List",
    "version": "0.1.0",
    "name": "List",
    "group": "Room",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "rules",
            "description": "<p>List of all rooms</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n   { \n   \"roomID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n   \"userID\":\"4ba83f3c-4ea4-4da4-9c06-e986a827230\",\n   \"number\":9,\n   \"occupied\":true\n   }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "get",
    "url": "/Room?RoomID",
    "title": "Read",
    "version": "0.1.0",
    "name": "Read",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Room identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "roomID",
            "description": "<p>Room identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "GUID",
            "optional": false,
            "field": "userID",
            "description": "<p>If room is occupied here will be id of client</p>"
          },
          {
            "group": "Success 200",
            "type": "Boolean",
            "optional": false,
            "field": "Occupied",
            "description": "<p>Is room free</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": false,
            "field": "Number",
            "description": "<p>Number of room</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n  { \n   \"roomID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n   \"userID\":\"4ba83f3c-4ea4-4da4-9c06-e986a827230\",\n   \"number\":9,\n   \"occupied\":false\n   }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of rooms</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "put",
    "url": "/Room?RoomID",
    "title": "Update",
    "version": "0.1.0",
    "name": "Update",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Room identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "Number",
            "description": "<p>Room number</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Room was updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"updated\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Room with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "post",
    "url": "/Task",
    "title": "Create",
    "version": "0.1.5",
    "name": "Create",
    "group": "Task",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "email",
            "description": "<p>User Email</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "numer",
            "description": "<p>Numer of room</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "title",
            "description": "<p>title of case</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>task was created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"created\"\n      }",
          "type": "json"
        }
      ]
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Title of case to be attached</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Email",
            "description": "<p>Email of issuer</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Describe",
            "description": "<p>Description of task (to be visualised for manager for ex.), can contain Description from Case and Number of Room</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "RoomNumber",
            "description": "<p>Number of room from which task is issued</p>"
          }
        ]
      }
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>User with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"userNotFound\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"roomNotFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "delete",
    "url": "/Task/TaskID",
    "title": "Delete",
    "version": "0.1.2",
    "name": "Delete",
    "group": "Task",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "TaskID",
            "description": "<p>Task identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Task was deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"removed\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Task with specified ID was not found</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "get",
    "url": "/Task",
    "title": "List",
    "version": "0.1.5",
    "name": "List",
    "group": "Task",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "tasks",
            "description": "<p>List of all existing tasks</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n  [\n      {\n                    \"taskID\": \"c49a6e23-e767-4904-b41a-4c31c3e80ac1\",\n                    \"describe\": \"Do something for me\",\n                    \"room\": {\n                      \"roomID\": \"d89fcd07-efba-4ee0-aa10-242c872454d1\",\n                      \"number\": \"1\",\n                      \"userID\": null,\n                      \"occupied\": false\n                    },\n                    \"issuer\": {\n                      \"userID\": \"377dcd34-bbff-4bdd-afc8-5e760ef1f1fd\",\n                      \"firstName\": \"Tom\",\n                      \"lastName\": \"Postman\",\n                      \"email\": \"guest1@hms.com\",\n                      \"room\": null\n                    },\n                    \"receiver\": {\n                      \"userID\": \"a03df2d1-f001-4848-973a-971033e5bb60\",\n                      \"firstName\": \"Alfons\",\n                      \"lastName\": \"Padlina\",\n                      \"email\": \"worker8@hms.com\",\n                      \"workerType\": \"Technician\"\n                    },\n                    \"listener\": null,\n                    \"case\": {\n                      \"caseID\": \"cee9dfb3-09f1-4846-aa32-3059ac6279e8\",\n                      \"title\": \"TestCase\",\n                      \"description\": \"Do something for me\",\n                      \"workerType\": 1,\n                      \"estimatedTime\": \"01:00:00\"\n                    },\n                    \"timeOfCreation\": \"2017-06-05T12:15:52.0584134\",\n                    \"status\": \"Done\",\n                    \"priority\": \"Emergency\"\n                  }\n  ]",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "get",
    "url": "/Task/Status",
    "title": "ListStatus",
    "version": "0.1.5",
    "name": "ListAvailableStatus",
    "group": "Task",
    "description": "<p>Gets all available statuses that can be assigned to task</p>",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "Names",
            "description": "<p>List of every status by name</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"names\":[\n         \"Unassigned\",\n                 \"Assigned\",\n                  \"Pending\",\n                  \"Done\"\n      ]\n      }",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "get",
    "url": "/Task/TaskID",
    "title": "Read",
    "version": "0.1.5",
    "name": "Read",
    "group": "Task",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "TaskID",
            "description": "<p>Task identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "taskID",
            "description": "<p>Task identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "describe",
            "description": "<p>of task</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "roomID",
            "description": "<p>Room identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "Room",
            "optional": false,
            "field": "room",
            "description": "<p>Room entity</p>"
          },
          {
            "group": "Success 200",
            "type": "Customer",
            "optional": false,
            "field": "issuer",
            "description": "<p>Customer entity</p>"
          },
          {
            "group": "Success 200",
            "type": "Worker",
            "optional": false,
            "field": "receiver",
            "description": "<p>Worker entity</p>"
          },
          {
            "group": "Success 200",
            "type": "Manager",
            "optional": false,
            "field": "listener",
            "description": "<p>Manager entity</p>"
          },
          {
            "group": "Success 200",
            "type": "Case",
            "optional": false,
            "field": "case",
            "description": "<p>Case entity</p>"
          },
          {
            "group": "Success 200",
            "type": "DateTime",
            "optional": false,
            "field": "timeOfCreation",
            "description": ""
          },
          {
            "group": "Success 200",
            "type": "Status",
            "optional": false,
            "field": "status",
            "description": "<p>Status of task</p>"
          },
          {
            "group": "Success 200",
            "type": "Priority",
            "optional": false,
            "field": "priority",
            "description": "<p>Priority of task</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n            \"taskID\": \"c49a6e23-e767-4904-b41a-4c31c3e80ac1\",\n            \"describe\": \"Do something for me\",\n            \"room\": {\n              \"roomID\": \"d89fcd07-efba-4ee0-aa10-242c872454d1\",\n              \"number\": \"1\",\n              \"userID\": null,\n              \"occupied\": false\n            },\n            \"issuer\": {\n              \"userID\": \"377dcd34-bbff-4bdd-afc8-5e760ef1f1fd\",\n              \"firstName\": \"Tom\",\n              \"lastName\": \"Postman\",\n              \"email\": \"guest1@hms.com\",\n              \"room\": null\n            },\n            \"receiver\": {\n              \"userID\": \"a03df2d1-f001-4848-973a-971033e5bb60\",\n              \"firstName\": \"Alfons\",\n              \"lastName\": \"Padlina\",\n              \"email\": \"worker8@hms.com\",\n              \"workerType\": \"Technician\"\n            },\n            \"listener\": null,\n            \"case\": {\n              \"caseID\": \"cee9dfb3-09f1-4846-aa32-3059ac6279e8\",\n              \"title\": \"TestCase\",\n              \"description\": \"Do something for me\",\n              \"workerType\": 1,\n              \"estimatedTime\": \"01:00:00\"\n            },\n            \"timeOfCreation\": \"2017-06-05T12:15:52.0584134\",\n            \"status\": \"Done\",\n            \"priority\": \"Emergency\"\n          }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of tasks</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "put",
    "url": "/task?TaskID",
    "title": "Update",
    "version": "0.1.5",
    "name": "Update",
    "group": "Task",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>task was updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"updated\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>task with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "post",
    "url": "/Task/Status/{TaskID}",
    "title": "UpdateStatus",
    "version": "0.1.5",
    "name": "UpdateStatus",
    "group": "Task",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "TaskID",
            "description": "<p>Task identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": false,
            "field": "Status",
            "description": "<p>Task status identifier - Unassigned, Assigned, Done, Pending</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Task status changed</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"StatusChanged\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Task with specified ID was not found</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>Status name is not correct</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 NotFound\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n  \"status\":\"InvalidStatus\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "get",
    "url": "/Worker/Shifts",
    "title": "ActualizeShifts",
    "version": "0.1.3",
    "name": "ActualizeShifts",
    "group": "Worker",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Shifts were updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"success\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Shifts are acutal - no need to update</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400 BadRequest\n{\n   \"status\":\"this action is not needed\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "HotelManagementSystem/Controllers/WorkerController.cs",
    "groupTitle": "Worker"
  },
  {
    "type": "get",
    "url": "/Worker/",
    "title": "List",
    "version": "0.1.3",
    "name": "List",
    "group": "Worker",
    "filename": "HotelManagementSystem/Controllers/WorkerController.cs",
    "groupTitle": "Worker"
  },
  {
    "type": "get",
    "url": "/Worker/{id}",
    "title": "Read",
    "version": "0.1.3",
    "name": "Read",
    "group": "Worker",
    "filename": "HotelManagementSystem/Controllers/WorkerController.cs",
    "groupTitle": "Worker"
  },
  {
    "type": "get",
    "url": "/Worker/{id}",
    "title": "Tasks",
    "version": "0.1.3",
    "name": "Read",
    "group": "Worker",
    "filename": "HotelManagementSystem/Controllers/WorkerController.cs",
    "groupTitle": "Worker"
  }
] });
