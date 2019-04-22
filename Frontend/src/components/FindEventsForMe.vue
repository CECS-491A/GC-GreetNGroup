<template>
    <v-app>
        <div class="Title">
            <h1>{{ title }}</h1>
        </div>
        <v-container fluid>
            <v-card ref="Tags">
                <v-layout align-start justify-start row wrap>
                    <v-flex xs5>
                        <v-checkbox v-model="useTags" label="Use Tags" value="UseTags"></v-checkbox>
                    </v-flex>
                </v-layout>
                <v-radio-group row>
                    <v-radio label="Outdoors" value="outdoors"></v-radio>
                    <v-radio label="Indoors" value="indoors"></v-radio>
                    <v-radio label="Music" value="music"></v-radio>
                    <v-radio label="Games" value="games"></v-radio>
                    <v-radio label="Fitness" value="fitness"></v-radio>
                    <v-radio label="Art" value="art"></v-radio>
                    <v-radio label="Sports" value="sports"></v-radio>
                    <v-radio label="Educational" value="educational"></v-radio>
                    <v-radio label="Food" value="food"></v-radio>
                    <v-radio label="Discussion" value="discussion"></v-radio>
                    <v-radio label="Miscellaneous" value="miscellaneous"></v-radio>
                </v-radio-group>
            </v-card>
            <hr>
            <v-card ref="Dates">
                <v-layout align-start justify-start row wrap>
                    <v-flex xs5>
                        <v-checkbox v-model="useDates" label="Use Dates" value="UseDates"></v-checkbox>
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
        </v-container>
    </v-app>
</template>

<script>
export default {
  name: 'FindEventsForMe',
  data () {
    return {
      title: 'Find Events For Me',
      useTags: '',
      useDates: '',
      startDate: new Date().toISOString().substr(0, 10),
      startDateMenu: false,
      endDate: new Date().toISOString().substr(0, 10),
      endDateMenu: false
    }
  }
}
</script>

<style scoped>
</style>
