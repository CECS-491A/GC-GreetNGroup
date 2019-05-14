<template>
<div class="EventPage">
  <h1 class='display-2'>{{this.message}}</h1>
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
          <v-btn color="success" v-on:click="joinEvent">Join Event</v-btn>
          <v-btn color="error" v-on:click="leaveEvent">Leave Event</v-btn>
        </v-card-actions>
        <div v-if="isAttendee">
<v-card>
          <input id="checkInBox" type="text" :disabled=attendeeCheck v-model="checkinCode" :maxlength=50 placeholder= 'CHECKIN CODE' />
          <v-btn color="attendee"
              v-on:click="checkIn">Check In</v-btn>
        </v-card>
        </div>
        
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
            <div style = "font-size: 15px; font-weight: bold;">Tags: <span v-for="(tag, index) in eventTags" v-bind:key="index">
            <span v-if="index != 0">, </span><span>{{ tag }}</span>
            </span></div>
          </div>
        </v-card-title>
      </v-card>
    </v-flex>
  </v-layout>
  </v-container>
  <v-expansion-panel style="maxWidth: 550px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;">
    <v-expansion-panel-content
      v-for="(item,i) in 1"
      :key="i"
    >
      <template v-slot:header>
        <div>Attendees</div>
      </template>
      <v-card style="width: 95%; text-decoration: none;">
        <li v-for="(value, index) in usersAttending"  v-bind:key="index" >
            {{value}}
            </li>
      </v-card>
    </v-expansion-panel-content>
  </v-expansion-panel>
</div>
</template>
<script>
import axios from 'axios'
import { apiURL } from '@/const.js'
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
      usersAttending: [],
      eventTags: [],
      jwt: localStorage.getItem('token'),
      checkinCode: '',
      isAttendee: null,
      checkedIn: null
    }
  },
  created () {
    axios({
      method: 'GET',
      url: `${apiURL}/event/info?name=` + this.eventNames,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      }
    })
      .then(response => (this.json = response.data))
      .catch(e => { this.errorMessage = e.response.data })
    if (localStorage.getItem('token') != null) {
      axios({
        method: 'POST',
        url: `${apiURL}/event/isAttendee`,
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          EventId: this.json.EventId,
          CheckinCode: this.checkinCode,
          JWT: this.jwt
        }
      })
        .then(response => {
          this.isAttendee = response.data
        })
    }
  },
  /*
  beforeUpdate () {
    axios({
      method: 'GET',
      url: `${apiURL}/user/username/` + this.json.UserId,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      }
    })
      .then(response => (this.userName = response.data))
      .catch(e => { this.errorMessage = e.response.data })
    axios({
      method: 'GET',
      url: `${apiURL}/attendee/` + this.json.EventId,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      }
    })
      .then(response => (this.usersAttending = response.data))
      .catch(e => { this.errorMessage = e.response.data })
    axios({
      method: 'GET',
      url: `${apiURL}/event/tags/` + this.json.EventId,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      }
    })
      .then(response => (this.eventTags = response.data))
      .catch(e => { this.errorMessage = e.response.data })
  },
  */
  methods: {
    checkAttendance: function () {
      axios({
        method: 'POST',
        url: `${apiURL}/event/isAttendee`,
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          EventId: this.json.EventId,
          CheckinCode: this.checkinCode,
          JWT: this.jwt
        }
      })
        .then(response => {
          this.isAttendee = response.data
        })
    },
    checkIn: function () {
      axios({
        method: 'POST',
        url: `${apiURL}/event/checkIn/`,
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          EventId: this.json.EventId,
          CheckinCode: this.checkinCode,
          JWT: this.jwt
        }
      })
        .then(response => {
          const isDataAvailable = response.data != null
          this.checkIn = isDataAvailable ? response.data : true
        })
    },
    joinEvent: function () {
      axios({
        method: 'POST',
        url: `${apiURL}/joinevent`,
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          jwtToken: this.jwt,
          eventID: this.json.EventId
        }
      })
        .then(response => (this.message = response.data), this.isAttendee = true)
        .catch(e => { this.errorMessage = e.response.data })
    },
    leaveEvent: function () {
      axios({
        method: 'POST',
        url: `${apiURL}/leaveevent`,
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          jwtToken: this.jwt,
          eventID: this.json.EventId
        }
      })
        .then(response => (this.message = response.data))
        .catch(e => { this.errorMessage = e.response.data })
    },
    formatDate (date) {
      // DateTime objects formatted as 'YYYY-MM-DD T HH:MM:SS', formatting will result in array of size 6
      var splitDate = date.split('-').join(',').split('T').join(',').split(':').join(',').split(',')
      var interval = parseInt(splitDate[3]) >= 12 ? 'PM' : 'AM'
      var hour = parseInt(splitDate[3], 10) % 12 !== 0 ? parseInt(splitDate[3], 10) % 12 : 12
      var formattedDate = splitDate[1] + '/' + splitDate[2] + '/' + splitDate[0] + ' ' + hour + ':' + splitDate[4] + interval
      return formattedDate
    }
  },
  computed: {
    attendeeCheck () {
      return !this.isAttendee
    }
  }
}
</script>
