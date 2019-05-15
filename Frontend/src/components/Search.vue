<template>
  <v-app>
    <div class="Search">
      <h1>{{ title }}</h1>
    </div>
    <v-container fluid>
      <v-layout align-start justify-center row wrap>
        <v-flex xs5>
          <input id="searchbar" type="text" v-model="search" :maxlength=50 placeholder= 'search' />
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
            <v-select
              v-model="newPageLimit"
              v-on:click="resetResults()"
              :items="resultCount"
              :menu-props="{ maxHeight: '200' }"
              label="Select display count"
              hide-details
            ></v-select>
      </v-flex>
      </v-layout>
      <v-layout align-start justify-center row wrap>
        <v-flex xs3>
          <div v-if="!checkSearchFilter(filter)">
            <div id="events-list">
              <h2>{{ errorInSearch }} </h2>
              <div v-if="events !== null">
                  <div id="events" v-for="{Uid, EventId, EventName, EventLocation, StartDate, index} in limitSearchResultsEvents" :key="index">
                    <v-card ref="Event">
                        <p>{{findUserByUserId(Uid)}}</p>
                        <router-link :to="'/event/' + EventId">
                        <button  id="event-b"> {{EventName}} </button>
                        </router-link>
                        <article> {{StartDate | moment("dddd, MMMM Do YYYY, h:mm a")}} </article>
                        <article> {{'Location: ' + EventLocation}} </article>
                        <article> {{'Host: ' + eventHost}} </article>
                    </v-card>
                </div>
              </div>
              <div v-else>{{ errorInSearch = 'Sorry! The we couldn\'t find anything!' }}</div>
            </div>
          </div>
          <div v-else>
            <div id="user-list">
              <h2>{{ errorInSearch }} </h2>
              <div v-if="user !== null">
                <div id="user-d" v-for="{Username, index} in limitSearchResultsUsers" :key="index">
                  <router-link :to="'/User/' + Username">
                    <button id="user-b" > {{Username}} </button>
                  </router-link>
                </div>
              </div>
              <div v-else>{{ errorInSearch = 'Sorry! The we couldn\'t find anything!' }}</div>
            </div>
          </div>
          <div>
            <v-btn small color="primary" dark 
            v-if="events.length > this.pageLimit"
            v-on:click.native="limitSearchResultsPrevious(events)">Previous</v-btn>
            <v-btn small color="primary" dark 
              v-if="events.length > this.pageLimit"
              v-on:click.native="limitSearchResultsNext(events)">Next</v-btn>
            <v-btn small color="primary" dark 
            v-if="user.length > this.pageLimit"
            v-on:click.native="limitSearchResultsPrevious(user)">Previous</v-btn>
            <v-btn small color="primary" dark 
              v-if="user.length > this.pageLimit"
              v-on:click.native="limitSearchResultsNext(user)">Next</v-btn>
          </div>
        </v-flex>
      </v-layout>
    </v-container>
  </v-app>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'

export default {
  name: 'SearchPage',
  data () {
    return {
      events: [],
      user: [],
      eventHost: [],
      title: 'GreetNGroup',
      placeholderText: 'search for events',
      search: '',
      eventName: '',
      errorInSearch: '',
      searchFilter: ['Users', 'Events'],
      resultCount: [5, 10, 15, 20, 40],
      filter: 'Events',
      pageturner: false,
      pageStart: 0,
      newPageLimit: 5,
      pageLimit: 5,
      pageEnd: 5,
      endReached: false,
      caughtError: ''
    }
  },
  methods: {
    resetResults: function () {
      this.events = []
      this.user = []
      this.pageStart = 0
      this.pageEnd = this.pageLimit
    },
    // Determines which field to search under
    checkSearchFilter: function (i) {
      if (i === 'Users') return true
      return false
    },
    // Determines if there is a search query
    checkInput: function (i) {
      if (i !== '') return true
      return false
    },
    // Return username given id in event search
    findUserByUserId: function (i) {
      var url = `${apiURL}/user/username/` + i
      axios.get(url)
        .then((response) => {
          const isDataAvailable = response.data && response.data.length > 0
          var name = isDataAvailable ? response.data : ''
          this.eventHost = name
        })
        .catch(error => console.log(error))
    },
    // Finds events by partial name match
    findEventsByName: function (i) {
      this.pageLimit = this.newPageLimit
      this.pageStart = 0
      this.pageEnd = this.pageLimit
      var url = `${apiURL}/searchEvent?name=`
      if (i === '') return
      axios.get(url + i)
        .then((response) => { 
          this.user = [] 
          const isDataAvailable = response.data && response.data.length > 0
          this.events = isDataAvailable ? response.data : []
          this.errorInSearch = isDataAvailable ? '' : 'Sorry! We could not find anything under that search at this time!'
        })
        .catch(error => console.log(error))
    },
    // Finds username by identical name match
    findUserByUsername: function (i) {
      this.pageLimit = this.newPageLimit
      this.pageStart = 0
      this.pageEnd = this.pageLimit
      var url = `${apiURL}/searchUser?username=`
      if (i === '') return
      axios.get(url + i)
        .then((response) => { 
          this.events = []
          const isDataAvailable = response.data
          this.user = isDataAvailable ? response.data : []
          this.errorInSearch = isDataAvailable ? '' : 'Sorry! could not find anything under that search at this time!'
        })
        .catch(error => console.log(error))
    },
    // Determines amount of results shown after next button press
    limitSearchResultsNext: function (i) {
      // if event list count is >= the starting count + limit of event listings allowed in a page
      if (!this.endReached) {
        if (i.length > this.pageEnd + this.pageLimit) { // if under the max
          this.pageStart += this.pageLimit
          this.pageEnd += this.pageLimit
        } else if (i.length <= this.pageEnd + this.pageLimit) { // if over the max
          this.endReached = true
          this.pageStart = this.pageEnd
          this.pageEnd = i.length
        }
      }
    },
    // Determines amount of result shown after previous button press
    limitSearchResultsPrevious: function (i) {
      this.endReached = false
      if (this.pageStart > 0) {
        if (this.pageStart - this.pageLimit >= 0) {
          this.pageEnd = this.pageStart
          this.pageStart -= this.pageLimit
        }
      }
    }
  },
  // Computes list to be displayed
  computed: {
    limitSearchResultsEvents () {
      return this.events.slice(this.pageStart, this.pageEnd)
    },
    limitSearchResultsUsers () {
      return this.user.slice(this.pageStart, this.pageEnd)
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
