<template>
  <v-app>
    <div class="Search">
      <h1>{{ title }}</h1>
    </div>
    <v-container fluid>
      <v-layout align-start justify-center row wrap>
        <v-flex xs5>
          <input id="searchbar" type="text" v-model="search" placeholder= 'search' />
          <button v-on:click="checkSearchFilter(filter) ? findUserByUsername(search) : findEventsByName(search)">Search</button>
        </v-flex>
      </v-layout>
      <v-layout align-start justify-center row wrap>
        <v-flex xs2>
            <v-select
              v-model="filter"
              :items="searchFilter"
              :menu-props="{ maxHeight: '200' }"
              label="Select a filter"
            ></v-select>
      </v-flex>
      </v-layout>
      <v-layout align-start justify-center row wrap>
        <v-flex xs3>
          <div v-if="!checkSearchFilter(filter)">
            <div id="events-list">
              <h2>{{ errorInSearch }} </h2>
              <div v-if="events !== null">
                  <div id="events" v-for="{UserId, User, EventId, StartDate, EventName, index} in limitSearchResults" :key="index">
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
          <div>
            <v-btn small color="primary" dark 
            v-if="events.length > 5"
            v-on:click.native="limitSearchResultsPrevious(events)">Previous</v-btn>
            <v-btn small color="primary" dark 
              v-if="events.length > 5"
              v-on:click.native="limitSearchResultsNext(events)">Next</v-btn>
          </div>
        </v-flex>
      </v-layout>
    </v-container>
  </v-app>
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
      searchFilter: ['Users', 'Events'],
      filter: 'Events',
      pageturner: false,
      pageStart: 0,
      pageEnd: 5,
      pageLimit: 5
    }
  },
  methods: {
    checkSearchFilter: function (i) {
      if (i === 'Users') return true
      return false
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
    },
    limitSearchResultsNext: function (i) {
      if (i.length >= this.pageStart + this.pageLimit) {
        // if over the max
        if (i.length < this.pageStart + this.pageLimit) {
          this.pageStart += this.pageLimit
          this.pageEnd = i.length
        } else if (i.length === this.pageStart + this.pageLimit) {
        } else {
          this.pageStart += this.pageLimit
          this.pageEnd += this.pageLimit
        }
      }
    },
    limitSearchResultsPrevious: function (i) {
      if (this.pageStart > 0) {
        if (this.pageStart - this.pageLimit >= 0) {
          this.pageEnd = this.pageStart
          this.pageStart -= this.pageLimit
        }
      }
    }
  },
  computed: {
    limitSearchResults () {
      return this.events.slice(this.pageStart, this.pageEnd)
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
#searchbar {
  width: 200px;
  height: 25px
}
</style>
