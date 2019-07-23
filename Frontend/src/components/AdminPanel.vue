<template>
  <div class="AdminPanel">
      <template>
        <div>
          <v-tabs
            v-model="active"
            color="rgba(66,133,244)"
            dark
            slider-color="yellow"
          >
          <v-tab v-for="item in items" :key="item">
            {{ item.title }}
          </v-tab>
            <v-tab-item v-for="item in items" :key="item">
              <v-card flat v-if="item.title=='User Analysis Dashboard'">
                <analysis-dashboard></analysis-dashboard>
              </v-card>
              <v-card flat v-if="item.title=='Search'">
                <search></search>
              </v-card>
            </v-tab-item>
          </v-tabs>
          </div>
        </template>
    </div>
</template>

<script>
import AnalysisDashboard from '@/components/AnalysisDashboard'
import Search from '@/components/Search'
export default {
  data () {
    return {
      active: null,
      items: [
        {title: 'User Analysis Dashboard'},
        {title: 'Search'}
      ]
    }
  },
  methods: {
  },
  components: {
    'analysis-dashboard': AnalysisDashboard,
    'search': Search
  }
}
</script>

<style scoped>
.AdminPanel {
  text-align: left;
  width: 70%;
  margin: 1px auto;
}
</style>
