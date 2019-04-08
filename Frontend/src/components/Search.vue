<template>
    <div>
        <div class="Search">
            <h1>{{ title }}</h1>
        </div>
        <div>
          <div>
            <input type="text" v-model="search" placeholder= 'search' />
            <input type="checkbox" id="userSearch" v-model="checked" v-on:click="userSearchFilter(checked)">
            <label for="checkbox">{{ 'User Search' }}</label>
          </div>
          <button v-on:click="checkUserSearchFilter() ? findUserByUsername(search) : findEventsByName(search)">Search</button>
          <div v-if="!checked">
            <div id="events-list">
              <h2>{{ errorInSearch }} </h2>
              <div v-if="events !== null">
                  <div id="events" v-for="{UserId, User, EventId, StartDate, EventName, index} in events" :key="index">
                    <button id="event-b"> {{EventName}} </button>
                    <article> {{'Start Date: ' + StartDate}} </article>
                  </div>
              </div>
              <div v-else>{{ errorInSearch = 'Sorry! The we couldn\'t find anything!' }}</div>
            </div>
          </div>
          <div v-else>
            <div id="user-list">
              <h2>{{ errorInSearch }} </h2>
              <div v-if="user !== null">
                <div id="user-d">
                   <button id="user-b"> {{user.UserName}} </button>
                </div>
              </div>
              <div v-else>{{ errorInSearch = 'Sorry! The we couldn\'t find anything!' }}</div>
            </div>
          </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'SearchPage',
  data () {
    return {
      events: [],
      user: {
        UserId: '',
        FirstName: '',
        LastName: '',
        UserName: '',
        City: '',
        State: '',
        Country: '',
        DoB: '',
        IsActivated: '',
        EventCreationCount: ''
      },
      title: 'GreetNGroup',
      placeholderText: 'search for events',
      search: '',
      eventName: '',
      errorInSearch: '',
      checked: false
    }
  },
  methods: {
    checkUserSearchFilter: function () {
      return this.checked
    },
    userSearchFilter: function (i) {
      if (i) this.searchForUser = true
      this.searchForUser = false
    },
    checkInput: function (i) {
      if (i !== '') return true
      return false
    },
    findEventsByName: function (i) {
      if (i === '') return
      axios.get('http://localhost:62008/api/searchEvent/' + i)// build version -> 'https://api.greetngroup.com/api/searchEvent/' + i)
        .then((response) => { 
          this.user = ''; const isDataAvailable = response.data && response.data.length > 0; this.events = isDataAvailable ? response.data : []; this.errorInSearch = isDataAvailable ? '' : 'Sorry! We could not find anything under that search at this time!'
        })
        .catch(error => console.log(error))
    },
    findUserByUsername: function (i) {
      if (i === '') return
      axios.get('http://localhost:62008/api/searchUser/' + i)// build version -> 'https://api.greetngroup.com/api/searchUser/' + i)
        .then((response) => { 
          this.events = []; const isDataAvailable = response.data; this.user = isDataAvailable ? response.data : ''; this.errorInSearch = isDataAvailable ? '' : 'Sorry! could not find anything under that search at this time!'
        })
        .catch(error => console.log(error))
    }
  }
}
</script>

<style scoped>
h1 {
    font-weight: normal;
}
#event-b, #user-b {
    font-weight: bold;
    font-size: 20px;
}
#events-list, h2 {
    margin: 25px;
}
#events {
    min-height: 100px;
}
</style>
