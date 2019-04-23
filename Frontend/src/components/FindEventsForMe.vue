<template>
    <v-app>
        <div class="Title">
            <h1>{{ title }}</h1>
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
                        @click="toggle"
                        >
                        <v-list-tile-action>
                            <v-icon :color="selectedTags.length > 0 ? 'indigo darken-4' : ''">{{ icon }}</v-icon>
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
                    @click="toggle"
                    >
                    <v-list-tile-action>
                        <v-icon :color="selectedStates.length > 0 ? 'indigo darken-4' : ''">{{ icon }}</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title>Select All</v-list-tile-title>
                    </v-list-tile-content>
                    </v-list-tile>
                    <v-divider class="mt-2"></v-divider>
                </template>
                </v-select>
            </v-card>
            <v-card>
                <button v-on:click="findEventsForMe()">Search</button>
            </v-card>
        </v-container>
    </v-app>
</template>

<script>
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
      selectedStates: []
    }
  },
  methods: {
    findEventsForMe: function () {
      return ''
    }
  }
}
</script>

<style scoped>
</style>
