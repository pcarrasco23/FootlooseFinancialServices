from __future__ import print_function

import json
import pymongo

def lambda_handler(event, context):
    # Open a connection to the Mongo instance on EC2
    client = pymongo.MongoClient("*******")

    # Get a reference to the footloosefs Mongo database
    db = client.footloosefs

    # The ID of the person we are updating will be in the subject
    personId = event['Records'][0]['Sns']['Subject']

    # The message will contain the JSON of the person attributes
    message = event['Records'][0]['Sns']['Message']
    
    print("From SNS: " + message)    
    print("From SNS: " + personId)  

    if personId:
        
         # Remove the document representing the person from the collection
        db.persons.remove( { "_id" : int(personId)  } )

        # Add the message with the updated person to the collection
        db.persons.insert_one( json.loads(message) )

        print("Processed insert or update for person: " + personId)

    return