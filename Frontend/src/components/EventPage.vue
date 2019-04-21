<template>
<div class="EventPage">
  <v-container fluid grid-list-md>
  <v-layout>
    <v-flex xs12 sm6 offset-sm3>
      <v-card>
        <v-card-title primary class="justify-center" >
          <div>
            <h3 class="headline mb-0" style = "font-size: 20px; text-decoration: underline;">{{this.json.EventName}}</h3>
            <h2>Host: {{this.userName}}</h2>
            <h2>Time: {{formatDate(this.json.StartDate) }}</h2>
            <h2>Location: {{this.json.EventLocation }}</h2>
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
            <h2>{{this.json.EventDescription }}</h2>
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
      userName: null,
      userID: null,
      json: {},
      userAttending: [],
      eventTAGS: []
    }
  },
  created () {
    axios({
      method: 'GET',
      url: 'http://localhost:62008/api/event/info?name=' + this.eventNames,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      }
    })
      .then(response => (this.json = response.data))
      .catch(e => { this.errorMessage = e.response.data })   
  },
  beforeUpdate () {
    axios({
      method: 'GET',
      url: 'http://localhost:62008/api/user/username/' + this.json.UserId,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      }
    })
      .then(response => (this.userName = response.data))
      .catch(e => { this.errorMessage = e.response.data })
  },
  methods: {
    joinEvent () {
    },
    leaveEvent () {
    },
    formatDate (date) {
      // DateTime objects formatted as 'YYYY-MM-DD T HH:MM:SS', formatting will result in array of size 6
      var splitDate = date.split('-').join(',').split('T').join(',').split(':').join(',').split(',')
      var interval = parseInt(splitDate[3]) >= 12 ? 'PM' : 'AM'
      var hour = parseInt(splitDate[3], 10) % 12 !== 0 ? parseInt(splitDate[3], 10) % 12 : 12
      var formattedDate = splitDate[1] + '/' + splitDate[2] + '/' + splitDate[0] + ' ' + hour + ':' + splitDate[4] + interval
      return formattedDate
    }
  }
}
</script>
