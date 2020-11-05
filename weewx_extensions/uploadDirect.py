import json

import weewx
from weewx.engine import StdService

class UploadDirect(StdService):
    """ Upload weather to a local db. """
    def __init__(self, engine, config_dict):
        super(UploadDirect, self).__init__(engine, config_dict)

        #service_dict = config_dict.get('UploadDirect', {})
        self.root_path = config_dict['WEEWX_ROOT']

        self.bind(weewx.NEW_ARCHIVE_RECORD, self.new_archive_record)
        #self.bind(weewx.NEW_LOOP_PACKET, self.new_loop_packet)

    def new_archive_record(self, event):
        data = event.record
        self.process(data)

    def new_loop_packet(self, event):
        data = event.packet
        self.process(data)

    def process(self, data):
        observation = {}
        observation['DateTime'] = int(data['dateTime'])
        observation['USUnits'] = int(data['usUnits'])
        observation['Interval'] = int(data['interval'])
        observation['Barometer'] = data['barometer']
        observation['Pressure'] = data['pressure']
        observation['Altimeter'] = data['altimeter']
        observation['OutsideTemperature'] = data['outTemp']
        observation['OutsideHumidity'] = data['outHumidity']
        observation['WindSpeed'] = data['windSpeed']
        observation['WindDirection'] = data['windDir']
        observation['WindGust'] = data['windGust']
        observation['WindGustDirection'] = data['windGustDir']
        observation['RainRate'] = data['rainRate']
        observation['Rain'] = data['rain']
        observation['DewPoint'] = data['dewpoint']
        observation['Windchill'] = data['windchill']
        observation['HeatIndex'] = data['heatindex']
        #observation['Evapotranspiration'] = data['ET']
        observation['Radiation'] = data['radiation']
        observation['Ultraviolet'] = data['UV']
        #observation['ExtraTemperature1'] = data['extraTemp1']
        #observation['ExtraTemperature2'] = data['extraTemp2']
        #observation['ExtraTemperature3'] = data['extraTemp3']
        #observation['SoilTemperature1'] = data['soilTemp1']
        #observation['SoilTemperature2'] = data['soilTemp2']
        #observation['SoilTemperature3'] = data['soilTemp3']
        #observation['SoilTemperature4'] = data['soilTemp4']
        #observation['LeafTemperature1'] = data['leafTemp1']
        #observation['LeafTemperature2'] = data['leafTemp2']
        #observation['ExtraHumidity1'] = data['extraHumid1']
        #observation['ExtraHumidity2'] = data['extraHumid2']
        #observation['SoilMoisture1'] = data['soilMoist1']
        #observation['SoilMoisture2'] = data['soilMoist2']
        #observation['SoilMoisture3'] = data['soilMoist3']
        #observation['SoilMoisture4'] = data['soilMoist4']
        #observation['LeafWetness1'] = data['leafWet1']
        #observation['LeafWetness2'] = data['leafWet2']
        observations = [observation]

        try:
            import os
            print(os.getcwd())
            location = self.root_path + "/InsertObservations"
            json_file = "temp.json"
            out_file = open(location + "/" + json_file, "w")
            json.dump(observations, out_file, indent=2)
            out_file.close()
            
            import subprocess
            p = subprocess.Popen(["dotnet", "run", "--json-file", json_file], cwd=location)
            p.wait()
        except Exception as e:
            print(e)

        print("done")
        # cd InsertObservations
        # dotnet run --jsonfile temp.json
        # cd ..

        
