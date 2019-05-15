<template>
    <v-app>
        <div class="Title">
            <h1>{{ this.title }}</h1>
        </div>
        <v-container fluid>
            <v-card ref="Tags">
                <v-layout align-start justify-start row wrap>
                    <v-flex xs5>
                        <v-checkbox v-model="useTags" label="Use Tags"></v-checkbox>
                    </v-flex>
                </v-layout>
                <v-select
                    v-model="selectedTags"
                    :items="tags"
                    label="Choose Tags"
                    multiple
                    >
                    <template v-slot:prepend-item>
                        <v-list-tile
                        ripple
                        >
                        <v-list-tile-action>
                            <v-icon :color="selectedTags.length > 0 ? 'indigo darken-4' : ''"></v-icon>
                        </v-list-tile-action>
                        <v-list-tile-content>
                            <v-list-tile-title>Select All</v-list-tile-title>
                        </v-list-tile-content>
                        </v-list-tile>
                        <v-divider class="mt-2"></v-divider>
                    </template>
                </v-select>
            </v-card>
            <hr>
            <v-card ref="Dates">
                <v-layout align-start justify-start row wrap>
                    <v-flex xs5>
                        <v-checkbox v-model="useDates" label="Use Dates"></v-checkbox>
                    </v-flex>
                </v-layout>
                <v-radio-group row>
                    <v-menu
                    ref="startDateMenu"
                    v-model="startDateMenu"
                    :close-on-content-click="false"
                    :nudge-right="40"
                    :return-value.sync="startDate"
                    lazy
                    transition="scale-transition"
                    offset-y
                    full-width
                    min-width="290px"
                    >
                    <template v-slot:activator="{ on }">
                        <v-text-field
                        v-model="startDate"
                        ref= "startDate"
                        label="Pick a Starting Date"
                        prepend-icon="event"
                        readonly
                        v-on="on"
                        ></v-text-field>
                    </template>
                    <v-date-picker v-model="startDate" no-title scrollable>
                        <v-spacer></v-spacer>
                        <v-btn flat color="primary" @click="startDateMenu = false">Cancel</v-btn>
                        <v-btn flat color="primary" @click="$refs.startDateMenu.save(startDate)">OK</v-btn>
                    </v-date-picker>
                    </v-menu>
                    <v-menu
                    ref="endDateMenu"
                    v-model="endDateMenu"
                    :close-on-content-click="false"
                    :nudge-right="40"
                    :return-value.sync="endDate"
                    lazy
                    transition="scale-transition"
                    offset-y
                    full-width
                    min-width="290px"
                    >
                    <template v-slot:activator="{ on }">
                        <v-text-field
                        v-model="endDate"
                        ref= "endDate"
                        label="Pick an Ending Date"
                        prepend-icon="event"
                        readonly
                        v-on="on"
                        ></v-text-field>
                    </template>
                    <v-date-picker v-model="endDate" no-title scrollable>
                        <v-spacer></v-spacer>
                        <v-btn flat color="primary" @click="endDateMenu = false">Cancel</v-btn>
                        <v-btn flat color="primary" @click="$refs.endDateMenu.save(endDate)">OK</v-btn>
                    </v-date-picker>
                    </v-menu>
                </v-radio-group>
            </v-card>
            <hr>
            <v-card ref="Locations">
                <v-layout align-start justify-start row wrap>
                    <v-flex xs5>
                        <v-checkbox v-model="useLocation" label="Use Location"></v-checkbox>
                    </v-flex>
                </v-layout>
                <v-select
                v-model="selectedStates"
                :items="states"
                label="Choose Location"
                >
                <template v-slot:prepend-item>
                    <v-list-tile
                    ripple
                    >
                    <v-list-tile-action>
                        <v-icon :color="selectedStates.length > 0 ? 'indigo darken-4' : ''"></v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title>Select a State</v-list-tile-title>
                    </v-list-tile-content>
                    </v-list-tile>
                    <v-divider class="mt-2"></v-divider>
                </template>
                </v-select>
            </v-card>
            <v-card>
                <v-select
                v-model="pageLimit"
                v-on:click="resetResults()"
                :items="resultCount"
                :menu-props="{ maxHeight: '200' }"
                label="Select display count"
                hide-details
                ></v-select>
            </v-card>
            <v-card>
                <v-btn large v-on:click="findEventsForMe()">Search</v-btn>
            </v-card>
        </v-container>
        <v-container fluid>
            <v-card ref="Events">
                <div id="events" v-for="{UserId, EventId, StartDate, EventName, EventLocation, EventDescription, index} in limitSearchResultsEvents" :key="index">
                    <v-card ref="Event">
                        <p>{{findUserByUserId(UserId)}}</p>
                        <router-link :to="'/eventpage/' + EventId">
                        <button  id="event-b"> {{EventName}} </button>
                        </router-link>
                        <article> {{StartDate | moment("dddd, MMMM Do YYYY, h:mm a")}} </article>
                        <article> {{'Location: ' + EventLocation}} </article>
                        <article> {{'Host: ' + eventHost}} </article>
                        <article> {{'Description: '}} </article>
                        <article> {{EventDescription}} </article>
                    </v-card>
                </div>
            </v-card>
        </v-container>
        <div>
            <v-btn small color="primary" dark 
                v-if="events.length > this.pageLimit"
                v-on:click.native="limitSearchResultsPrevious(events)">Previous</v-btn>
            <article> {{this.currentPage + '/' + this.pageCount}} </article>
            <v-btn small color="primary" dark 
                v-if="events.length > this.pageLimit"
                v-on:click.native="limitSearchResultsNext(events)">Next</v-btn>
        </div>
    </v-app>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'

export default {
  name: 'FindEventsForMe',
  data () {
    return {
      title: 'Find Events For Me',
      useTags: false,
      useDates: false,
      useLocation: false,
      startDate: new Date().toISOString().substr(0, 10),
      startDateMenu: false,
      endDate: new Date().toISOString().substr(0, 10),
      endDateMenu: false,
      events: [],
      eventHost: [],
      tags: [
        'Outdoors', 'Indoors', 'Music', 'Games', 'Fitness', 'Art', 'Sports', 'Educational', 'Food',
        'Discussion', 'Miscellaneous'
      ],
      selectedTags: [],
      states: [
        'Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California', 'Colorado', 'Connecticut', 
        'Delaware', 'Florida', 'Georgia', 'Hawaii', 'Idaho', 'Illinois Indiana', 'Iowa', 'Kansas', 
        'Kentucky', 'Louisiana', 'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota',
        'Mississippi', 'Missouri', 'Montana Nebraska', 'Nevada', 'New Hampshire', 'New Jersey',
        'New Mexico', 'New York', 'North Carolina', 'North Dakota', 'Ohio', 'Oklahoma', 'Oregon',
        'Pennsylvania Rhode Island', 'South Carolina', 'South Dakota', 'Tennessee', 'Texas', 'Utah',
        'Vermont', 'Virginia', 'Washington', 'West Virginia', 'Wisconsin', 'Wyoming'
      ],
      selectedStates: '',
      resultCount: [5, 10, 15, 20, 40],
      pageStart: 0,
      newPageLimit: 5,
      pageLimit: 5,
      pageEnd: 5,
      currentPage: 1
    }
  },
  methods: {
    resetResults: function () {
      this.events = []
      this.pageStart = 0
      this.pageEnd = this.pageLimit
    },
    findEventsForMe: function () {
      this.pageStart = 0
      this.currentPage = 1
      this.pageEnd = this.pageLimit
      var url = `${apiURL}/FindEventsForMe`
      axios.post(url, {
        UseTags: this.useTags,
        UseDates: this.useDates,
        UseLocation: this.useLocation,
        Tags: this.selectedTags,
        StartDate: this.startDate,
        EndDate: this.endDate,
        State: this.selectedStates
      }).then((response) => {
        const isDataAvailable = response.data && response.data.length > 0
        this.events = isDataAvailable ? response.data : []
      })
    },
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
    // Determines amount of results shown after next button press
    limitSearchResultsNext: function (i) {
      // if event list count is >= the starting count + limit of event listings allowed in a page
      if (!this.endReached) {
        if (i.length > this.pageEnd + this.pageLimit) { // if under the max
          this.currentPage += 1
          this.pageStart += this.pageLimit
          this.pageEnd += this.pageLimit
        } else if (i.length <= this.pageEnd + this.pageLimit) { // if over the max
          this.currentPage += 1
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
          this.currentPage -= 1
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
    pageCount () {
      var pageCount = Math.round(this.events.length / this.pageLimit)
      if (pageCount === 0) pageCount = 1
      return pageCount
    }
  }
}
</script>

<style scoped>
</style>
