<template>
<div class="EventPage">
  <v-container fluid grid-list-md>
  <v-layout>
    <v-flex xs12 sm6 offset-sm3>
      <v-card>
        <v-card-title primary class="justify-center" >
          <div>
            <h3 class="headline mb-0" style = "font-size: 20px; text-decoration: underline;">{{this.json[0].EventName}}</h3>
            <h2>Host: {{this.json[0].User}}</h2>
            <h2>Time: {{this.json[0].StartDate }}</h2>
            <h2>Location: {{this.json[0].EventLocation }}</h2>
          </div>
        </v-card-title>
        <v-card-actions primary class="justify-center">
          <v-btn color="success" @click="joinEvent()">Join Event</v-btn>
          <v-btn color="error" @click="leaveEvent()">Leave Event</v-btn>
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
  </v-container>

  <v-container fluid grid-list-md>
  <v-layout>
    <v-flex xs12 sm6 offset-sm3>
      <v-card>
        <v-card-title primary class="justify-center">
          <div>
            <h3 class="headline mb-0" style = "font-size: 20px; text-decoration: underline;">Description:</h3>
            <h2>{{this.json[0].EventDescription }}</h2>
          </div>
        </v-card-title>
      </v-card>
    </v-flex>
  </v-layout>
  </v-container>

  <v-container fluid grid-list-md>
  <v-layout>
    <v-flex xs12 sm6 offset-sm3>
      <v-card>
        <v-card-title primary class="justify-center" style = "font-size: 20px; text-decoration: underline;">
          <div>
            <h3 class="headline mb-0" >Attendees:</h3>
          </div>
        </v-card-title>
      </v-card>
    </v-flex>
  </v-layout>
  </v-container>
</div>
</template>
<script>
import axios from 'axios'
// import { apiURL } from '@/const.js'
export default {
  name: 'Profile',
  data () {
    return {
      eventRetrieved: false,
      message: null,
      errorMessage: null,
      eventNames: this.$route.params.name,
      json: [],
      userAttending: [],
      eventTAGS: []
    }
  },
  created () {
    axios.get('http://localhost:62008/api/searchEvent?name=' + this.eventNames) // build version -> 'https://api.greetngroup.com/api/searchEvent?name=' + i)
      .then((response) => { 
        this.user = '' 
        const isDataAvailable = response.data && response.data.length > 0
        this.json = isDataAvailable ? response.data : []
      })
      .catch(error => console.log(error))
  },
  methods: {
    joinEvent () {
    },
    leaveEvent () {
    }
  }
}
</script>
